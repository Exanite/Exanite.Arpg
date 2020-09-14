using System;

namespace Prototype.Movement
{
    public class RingBuffer<T>
    {
        private readonly T[] array;

        private int bitmask;

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
                return array[index & bitmask];
            }

            set
            {
                array[index & bitmask] = value;
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

            this[write++] = value;
        }

        public T Dequeue()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Buffer is empty. Cannot Dequeue an item.");
            }

            return this[read++];
        }

        public bool TryDequeue(out T value)
        {
            if (!IsEmpty)
            {
                value = this[read++];
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
