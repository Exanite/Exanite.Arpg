using System;
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
        /// Reads a <see cref="Vector3"/> (12 bytes)
        /// </summary>
        public static Vector3 GetVector3(this NetDataReader reader)
        {
            return new Vector3(reader.GetFloat(), reader.GetFloat(), reader.GetFloat());
        }

        /// <summary>
        /// Reads a <see cref="Vector2"/> (8 bytes)
        /// </summary>
        public static Vector2 GetVector2(this NetDataReader reader)
        {
            return new Vector2(reader.GetFloat(), reader.GetFloat());
        }

        /// <summary>
        /// Reads a <see cref="Quaternion"/> (12 bytes)
        /// </summary>
        public static Quaternion GetQuaternion(this NetDataReader reader)
        {
            float x = reader.GetFloat();
            float y = reader.GetFloat();
            float z = reader.GetFloat();
            float w = Mathf.Sqrt(1f - (x * x + y * y + z * z));

            return new Quaternion(x, y, z, w);
        }

        /// <summary>
        /// Reads a <see cref="Guid"/> (16 bytes)
        /// </summary>
        public static Guid GetGuid(this NetDataReader reader)
        {
            return new Guid(reader.GetBytesWithLength());
        }

        /// <summary>
        /// Writes a <see cref="Vector3"/> (12 bytes)
        /// </summary>
        public static void Put(this NetDataWriter writer, Vector3 value)
        {
            writer.Put(value.x);
            writer.Put(value.y);
            writer.Put(value.z);
        }

        /// <summary>
        /// Writes a <see cref="Vector2"/> (8 bytes)
        /// </summary>
        public static void Put(this NetDataWriter writer, Vector2 value)
        {
            writer.Put(value.x);
            writer.Put(value.y);
        }

        /// <summary>
        /// Writes a <see cref="Quaternion"/> (12 bytes)
        /// </summary>
        public static void Put(this NetDataWriter writer, Quaternion value)
        {
            // (x * x) + (y * y) + (z * z) + (w * w) = 1 => No need to send w
            writer.Put(value.x);
            writer.Put(value.y);
            writer.Put(value.z);
        }

        /// <summary>
        /// Writes a <see cref="Guid"/> (16 bytes)
        /// </summary>
        public static void Put(this NetDataWriter writer, Guid value)
        {
            writer.PutBytesWithLength(value.ToByteArray());
        }
    }
}
