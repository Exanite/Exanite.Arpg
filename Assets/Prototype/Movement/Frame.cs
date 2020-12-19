namespace Prototype.Movement
{
    public struct Frame<T>
    {
        public uint tick;
        public T data;

        public Frame(uint tick, T value)
        {
            this.tick = tick;
            this.data = value;
        }
    }
}
