using System;

namespace Exanite.Arpg.AssetRegistry.Providers
{
    [Serializable]
    public class InvalidAssetTypeException : Exception
    {
        public Type Expected { get; }
        public Type Actual { get; }

        public InvalidAssetTypeException(string message, Type expected, Type actual) : base(message)
        {
            Expected = expected;
            Actual = actual;
        }

        public InvalidAssetTypeException(string message, Type expected, Type actual, Exception innerException) : base(message, innerException)
        {
            Expected = expected;
            Actual = actual;
        }
    }
}
