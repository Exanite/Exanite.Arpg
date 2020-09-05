using LiteNetLib.Utils;
using UnityEngine;

namespace Exanite.Arpg.Networking
{
    /// <summary>
    /// Extensions that simplify reading and writing to <see cref="NetDataReader"/> and <see cref="NetDataWriter"/>
    /// </summary>
    public static class NetSerializationExtensions
    {
        // Methods are from the link below, adapted to work for LiteNetLib
        // https://github.com/LukeStampfli/DarkriftSerializationExtensions/blob/master/DarkriftSerializationExtensions/DarkriftSerializationExtensions/SerializationExtensions.cs

        /// <summary>
        /// Writes a Vector3 (12 bytes)
        /// </summary>
        public static void Put(this NetDataWriter writer, Vector3 value)
        {
            writer.Put(value.x);
            writer.Put(value.y);
            writer.Put(value.z);
        }

        /// <summary>
        /// Writes a Vector2 (8 bytes)
        /// </summary>
        public static void Put(this NetDataWriter writer, Vector2 value)
        {
            writer.Put(value.x);
            writer.Put(value.y);
        }

        /// <summary>
        /// Writes a Quaternion (12 bytes)
        /// </summary>
        public static void Put(this NetDataWriter writer, Quaternion q)
        {
            // (x * x) + (y * y) + (z * z) + (w * w) = 1 => No need to send w
            writer.Put(q.x);
            writer.Put(q.y);
            writer.Put(q.z);
        }

        /// <summary>
        /// Reads a Vector3 (12 bytes)
        /// </summary>
        public static Vector3 GetVector3(this NetDataReader reader)
        {
            return new Vector3(reader.GetFloat(), reader.GetFloat(), reader.GetFloat());
        }

        /// <summary>
        /// Reads a Vector2 (8 bytes)
        /// </summary>
        public static Vector2 GetVector2(this NetDataReader reader)
        {
            return new Vector2(reader.GetFloat(), reader.GetFloat());
        }

        /// <summary>
        /// Reads a Quaternion (12 bytes)
        /// </summary>
        public static Quaternion GetQuaternion(this NetDataReader reader)
        {
            float x = reader.GetFloat();
            float y = reader.GetFloat();
            float z = reader.GetFloat();
            float w = Mathf.Sqrt(1f - (x * x + y * y + z * z));

            return new Quaternion(x, y, z, w);
        }
    }
}
