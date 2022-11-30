namespace System
{
    public class CollectionItemEventArgs : EventArgs
    {
        public int Index { get; }

        public object Value { get; }

        public CollectionItemEventArgs(int index, object value)
        {
            Index = index;
            Value = value;
        }
    }

    public class CollectionItemEventArgs<T> : EventArgs
    {
        public int Index { get; }

        public T Value { get; }

        public CollectionItemEventArgs(int index, T value)
        {
            Index = index;
            Value = value;
        }
    }

    public class CollectionItemMoveEventArgs : EventArgs
    {
        public int FromIndex { get; }

        public int ToIndex { get; }

        public CollectionItemMoveEventArgs(int fromIndex, int toIndex)
        {
            FromIndex = fromIndex;
            ToIndex = toIndex;
        }
    }

    public class ValueChangedEventArgs<T> : EventArgs
    {
        public T OldValue { get; }

        public T NewValue { get; }

        public ValueChangedEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    public class ExceptionEventArgs : EventArgs
    {
        public Exception Exception { get; }

        public ExceptionEventArgs(Exception exception)
        {
            Exception = exception;
        }
    }

    public class AttributeChangedEventArgs<T> : EventArgs
    {
        public T Value { get; }

        public AttributeChangedEventArgs(T value)
        {
            Value = value;
        }
    }
}
