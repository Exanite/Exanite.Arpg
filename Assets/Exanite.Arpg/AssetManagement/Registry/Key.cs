using System;

namespace Exanite.Arpg.AssetManagement.Registry
{
    public struct Key : IEquatable<Key>
    {
        private readonly string value;

        public string Value
        {
            get
            {
                return value;
            }
        }

        public Key(string value)
        {
            this.value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public static implicit operator Key(string value)
        {
            return new Key(value);
        }

        public static bool operator ==(Key lhs, Key rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Key lhs, Key rhs)
        {
            return !lhs.Equals(rhs);
        }

        public bool Equals(Key other)
        {
            return value.Equals(other.value, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            if (obj is Key key)
            {
                return Equals(key);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public override string ToString()
        {
            return value;
        }
    }
}
