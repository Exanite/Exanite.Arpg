using Prototype.Networking.Players.Packets;
using UnityEngine;

namespace Prototype.Networking.Players
{
    public class PlayerMovementBehaviour : MonoBehaviour
    {
        public const float MapSize = 10; // ! temp

        public PlayerInputPacket input = new PlayerInputPacket();

        private void FixedUpdate()
        {
            transform.position += (Vector3)(input.movement * Time.deltaTime * 5);

            WrapPosition();
        }

        private void WrapPosition()
        {
            Vector2 position = transform.position;

            position.x = Wrap(position.x, -MapSize, MapSize);
            position.y = Wrap(position.y, -MapSize, MapSize);

            transform.position = position;
        }

        /// <summary>
        /// Wraps a value between min and max values
        /// </summary>
        public static float Wrap(float value, float min, float max)
        {
            return Modulo(value - min, max - min) + min;
        }

        /// <summary>
        /// Returns the true modulo of a value when divided by a divisor
        /// </summary>
        public static float Modulo(float value, float divisor)
        {
            return ((value % divisor) + divisor) % divisor;
        }
    }
}
