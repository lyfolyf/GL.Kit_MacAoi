using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace GL.Kit.Net.Sockets
{
    public class _SocketAsyncServer
    {
        readonly int m_maxClientCount;  // 客户端最大连接数
        readonly Semaphore m_Semaphore; // 用于限制连接数量

        int m_connectedClientCount;     // 当前连接数

        readonly SocketAsyncEventArgsPool m_socketArgsPool;
        readonly BufferManager m_bufferManager;

        const int opsToPreAlloc = 2;

        int m_totalBytesRead;

        Socket listenSocket;

        public _SocketAsyncServer(int maxClientCount, int receiveBufferSize)
        {
            m_totalBytesRead = 0;
            m_connectedClientCount = 0;
            m_maxClientCount = maxClientCount;

            m_bufferManager = new BufferManager(receiveBufferSize * maxClientCount * opsToPreAlloc, receiveBufferSize);

            m_socketArgsPool = new SocketAsyncEventArgsPool(maxClientCount);
            m_Semaphore = new Semaphore(maxClientCount, maxClientCount);
        }

        public void Init()
        {
            m_bufferManager.InitBuffer();

            SocketAsyncEventArgs readWriteEventArg;

            for (int i = 0; i < m_maxClientCount; i++)
            {
                readWriteEventArg = new SocketAsyncEventArgs();
                readWriteEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                readWriteEventArg.UserToken = new AsyncUserToken();

                m_bufferManager.SetBuffer(readWriteEventArg);

                m_socketArgsPool.Push(readWriteEventArg);
            }
        }

        public void Start(IPEndPoint localEndPoint)
        {
            listenSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.Bind(localEndPoint);

            listenSocket.Listen(100);

            StartAccept(null);
        }

        public void StartAccept(SocketAsyncEventArgs acceptEventArg)
        {
            if (acceptEventArg == null)
            {
                acceptEventArg = new SocketAsyncEventArgs();
                acceptEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(AcceptEventArg_Completed);
            }
            else
            {
                acceptEventArg.AcceptSocket = null;
            }

            // 当达到最大接入量的时候，阻止新的连接
            m_Semaphore.WaitOne();

            bool willRaiseEvent = listenSocket.AcceptAsync(acceptEventArg);
            if (!willRaiseEvent)
            {
                ProcessAccept(acceptEventArg);
            }
        }

        void AcceptEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            Interlocked.Increment(ref m_connectedClientCount);
            Console.WriteLine($"连接接入：{e.RemoteEndPoint}");

            SocketAsyncEventArgs readEventArgs = m_socketArgsPool.Pop();
            ((AsyncUserToken)readEventArgs.UserToken).Socket = e.AcceptSocket;

            bool willRaiseEvent = e.AcceptSocket.ReceiveAsync(readEventArgs);
            if (!willRaiseEvent)
            {
                ProcessReceive(readEventArgs);
            }

            StartAccept(e);
        }

        void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceive(e);
                    break;
                case SocketAsyncOperation.Send:
                    ProcessSend(e);
                    break;
                default:
                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }
        }

        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            AsyncUserToken token = (AsyncUserToken)e.UserToken;
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                Interlocked.Add(ref m_totalBytesRead, e.BytesTransferred);
                Console.WriteLine("The server has read a total of {0} bytes", m_totalBytesRead);

                e.SetBuffer(e.Offset, e.BytesTransferred);
                bool willRaiseEvent = token.Socket.SendAsync(e);
                if (!willRaiseEvent)
                {
                    ProcessSend(e);
                }
            }
            else
            {
                CloseClientSocket(e);
            }
        }

        private void ProcessSend(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                AsyncUserToken token = (AsyncUserToken)e.UserToken;

                bool willRaiseEvent = token.Socket.ReceiveAsync(e);
                if (!willRaiseEvent)
                {
                    ProcessReceive(e);
                }
            }
            else
            {
                CloseClientSocket(e);
            }
        }

        private void CloseClientSocket(SocketAsyncEventArgs e)
        {
            AsyncUserToken token = e.UserToken as AsyncUserToken;

            try
            {
                token.Socket.Shutdown(SocketShutdown.Send);
            }
            catch (Exception) { }

            token.Socket.Close();

            Interlocked.Decrement(ref m_connectedClientCount);

            m_socketArgsPool.Push(e);

            m_Semaphore.Release();
            Console.WriteLine("A client has been disconnected from the server. There are {0} clients connected to the server", m_connectedClientCount);
        }
    }

    class SocketAsyncEventArgsPool
    {
        readonly Stack<SocketAsyncEventArgs> m_pool;

        public SocketAsyncEventArgsPool(int capacity)
        {
            m_pool = new Stack<SocketAsyncEventArgs>(capacity);
        }

        public void Push(SocketAsyncEventArgs item)
        {
            if (item == null) throw new ArgumentNullException("Items added to a SocketAsyncEventArgsPool cannot be null");
            lock (m_pool)
            {
                m_pool.Push(item);
            }
        }

        public SocketAsyncEventArgs Pop()
        {
            lock (m_pool)
            {
                return m_pool.Pop();
            }
        }

        public int Count
        {
            get { return m_pool.Count; }
        }
    }

    class BufferManager
    {
        int m_numBytes;                 // the total number of bytes controlled by the buffer pool
        byte[] m_buffer;                // the underlying byte array maintained by the Buffer Manager
        Stack<int> m_freeIndexPool;     //
        int m_currentIndex;
        int m_bufferSize;

        public BufferManager(int totalBytes, int bufferSize)
        {
            m_numBytes = totalBytes;
            m_currentIndex = 0;
            m_bufferSize = bufferSize;
            m_freeIndexPool = new Stack<int>();
        }

        public void InitBuffer()
        {
            m_buffer = new byte[m_numBytes];
        }

        public bool SetBuffer(SocketAsyncEventArgs args)
        {
            if (m_freeIndexPool.Count > 0)
            {
                args.SetBuffer(m_buffer, m_freeIndexPool.Pop(), m_bufferSize);
            }
            else
            {
                if ((m_numBytes - m_bufferSize) < m_currentIndex)
                {
                    return false;
                }
                args.SetBuffer(m_buffer, m_currentIndex, m_bufferSize);
                m_currentIndex += m_bufferSize;
            }
            return true;
        }

        public void FreeBuffer(SocketAsyncEventArgs args)
        {
            m_freeIndexPool.Push(args.Offset);
            args.SetBuffer(null, 0, 0);
        }
    }

    class AsyncUserToken
    {
        public Socket Socket { get; set; }
    }
}
