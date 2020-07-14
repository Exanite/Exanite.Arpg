using DarkRift;
using UnityEngine;

namespace Exanite.Arpg.Networking
{
    /// <summary>
    /// Extensions that simplify reading and writing DarkRift serialized data
    /// </summary>
    public static class DarkRiftSerializationExtensions
    {
        // Methods are from https://github.com/LukeStampfli/DarkriftSerializationExtensions/blob/master/DarkriftSerializationExtensions/DarkriftSerializationExtensions/SerializationExtensions.cs

        /// <summary>
        /// Writes a Vector3 (12 bytes)
        /// </summary>
        public static void WriteVector3(this DarkRiftWriter writer, Vector3 value)
        {
            writer.Write(value.x);
            writer.Write(value.y);
            writer.Write(value.z);
        }

        /// <summary>
        /// Reads a Vector3 (12 bytes)
        /// </summary>
        public static Vector3 ReadVector3(this DarkRiftReader reader)
        {
            return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        /// <summary>
        /// Writes a Vector2 (8 bytes)
        /// </summary>
        public static void WriteVector2(this DarkRiftWriter writer, Vector2 value)
        {
            writer.Write(value.x);
            writer.Write(value.y);
        }

        /// <summary>
        /// Reads a Vector2 (8 bytes)
        /// </summary>
        public static Vector2 ReadVector2(this DarkRiftReader reader)
        {
            return new Vector2(reader.ReadSingle(), reader.ReadSingle());
        }

        /// <summary>
        /// Writes a Quaternion (12 bytes)
        /// </summary>
        public static void WriteQuaternion(this DarkRiftWriter writer, Quaternion q)
        {
            // (x * x) + (y * y) + (z * z) + (w * w) = 1 => No need to send w
            writer.Write(q.x);
            writer.Write(q.y);
            writer.Write(q.z);
        }

        /// <summary>
        /// Reads a Quaternion (12 bytes)
        /// </summary>
        public static Quaternion ReadQuaternion(this DarkRiftReader reader)
        {
            float x = reader.ReadSingle();
            float y = reader.ReadSingle();
            float z = reader.ReadSingle();
            float w = Mathf.Sqrt(1f - (x * x + y * y + z * z));

            return new Quaternion(x, y, z, w);
        }
    }
}
