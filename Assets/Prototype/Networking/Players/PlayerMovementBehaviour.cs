using Prototype.Networking.Players.Packets;
using UnityEngine;

namespace Prototype.Networking.Players
{
    public class PlayerMovementBehaviour : MonoBehaviour
    {
        public const float MapSize = 10; // ! temp

        public PlayerInputPacket input = new PlayerInputPacket();

        private PlayerCharacter character;

        private void Start()
        {
            character = GetComponent<PlayerCharacter>();
        }

        private void FixedUpdate()
        {
            Tick(Time.fixedDeltaTime, Time.fixedTime);
        }

        public void Tick(float deltaTime, float currentTime)
        {
            Vector2 position = character.currentPosition;
            position += input.movement * deltaTime * 5;
            position = Wrap(position);

            character.UpdatePosition(position, currentTime);
        }

        private static Vector2 Wrap(Vector2 position)
        {
            position.x = Wrap(position.x, -MapSize, MapSize);
            position.y = Wrap(position.y, -MapSize, MapSize);

            return position;
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
