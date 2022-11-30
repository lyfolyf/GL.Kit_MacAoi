using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GL.Kit.Log
{
    public class LogSimulateTest
    {
        readonly LogPublisher m_log;

        int m_TaskCount;
        List<Task> tasks;
        CancellationTokenSource cts;

        public LogSimulateTest(LogPublisher log, int taskCount)
        {
            m_log = log;
            m_TaskCount = taskCount;
        }

        public void Start()
        {
            tasks = new List<Task>(m_TaskCount);

            cts = new CancellationTokenSource();

            for (int i = 0; i < m_TaskCount; i++)
            {
                string taskName = $"【任务 {i}】";
                Task task = new Task(() => LogTaskMethod(m_log, taskName, cts.Token), cts.Token, TaskCreationOptions.LongRunning);

                tasks.Add(task);
            }

            tasks.ForEach((t) => t.Start());
        }

        public void Close()
        {
            cts.Cancel();
        }

        private void LogTaskMethod(LogPublisher log, string name, CancellationToken token)
        {
            int infoCount = 0;
            int warnCount = 0;
            int errorCount = 0;

            while (!token.IsCancellationRequested)
            {
                int r = new Random().Next(1, 101);
                if (r <= 50)
                {
                    log.Info($"{name} 消息 {infoCount++}");
                }
                else if (r <= 80)
                {
                    log.Warn($"{name} 警告 {warnCount++}");
                }
                else
                {
                    log.Error($"{name} 错误 {errorCount++}");
                }

                int sleepTime = new Random().Next(1, 1001);
                Thread.Sleep(sleepTime);
            }
        }
    }
}
