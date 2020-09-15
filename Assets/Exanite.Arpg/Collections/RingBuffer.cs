using System;

namespace Exanite.Arpg.Collections
{
    public class RingBuffer<T>
    {
        private readonly T[] array;
        private readonly int bitmask;

        private int read;
        private int write;

        public RingBuffer(int capacity)
        {
            capacity = GetNextPowerOfTwo(capacity);
            array = new T[capacity];

            bitmask = capacity - 1;
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index), "Index was out of range. Must be non-negative and less than the size of the collection");
                }

                return array[bitmask & (read + index)];
            }

            set
            {
                if (index < 0 || index >= Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index), "Index was out of range. Must be non-negative and less than the size of the collection");
                }

                array[bitmask & (read + index)] = value;
            }
        }

        public int Capacity
        {
            get
            {
                return array.Length;
            }
        }

        public int Count
        {
            get
            {
                return write - read;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return read == write;
            }
        }

        public bool IsFull
        {
            get
            {
                return Count == Capacity;
            }
        }

        public void Enqueue(T value)
        {
            if (IsFull)
            {
                throw new InvalidOperationException("Buffer is full. Cannot Queue a new item.");
            }

            array[bitmask & write++] = value;
        }

        public T Dequeue()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Buffer is empty. Cannot Dequeue an item.");
            }

            return array[bitmask & read++];
        }

        public bool TryDequeue(out T value)
        {
            if (!IsEmpty)
            {
                value = array[bitmask & read++];
                return true;
            }

            value = default;
            return false;
        }

        public T Peek()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Buffer is empty. Cannot Peek an item.");
            }

            return array[bitmask & read];
        }

        public bool TryPeek(out T value)
        {
            if (!IsEmpty)
            {
                value = array[bitmask & read];
                return true;
            }

            value = default;
            return false;
        }

        private int GetNextPowerOfTwo(int value)
        {
            int result = 2;
            while (result < value)
            {
                result <<= 1;
            }

            return result;
        }
    }
}
