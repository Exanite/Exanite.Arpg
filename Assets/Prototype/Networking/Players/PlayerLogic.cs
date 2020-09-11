using Prototype.Networking.Players.Data;
using Prototype.Networking.Zones;
using UnityEngine;
using Zenject;

namespace Prototype.Networking.Players
{
    public class PlayerLogic : MonoBehaviour
    {
        public const float MapSize = 10; // ! temp

        public PlayerInputData input;

        private Zone zone;

        [Inject]
        public void Inject(Player player, Zone zone)
        {
            if (!(player.IsLocal || player.IsServer))
            {
                enabled = false;
            }

            this.zone = zone;
        }

        public PlayerUpdateData Simulate(PlayerUpdateData updateData, PlayerInputData inputData)
        {
            updateData.playerPosition += (Vector3)input.movement * zone.TimePerTick * 5;
            updateData.playerPosition = Wrap(updateData.playerPosition);

            return updateData;
        }

        private static Vector2 Wrap(Vector2 position)
        {
            position.x = Wrap(position.x, -MapSize, MapSize);
            position.y = Wrap(position.y, -MapSize, MapSize);

            return position;
        }

        private static float Wrap(float value, float min, float max)
        {
            return Modulo(value - min, max - min) + min;
        }

        private static float Modulo(float value, float divisor)
        {
            return ((value % divisor) + divisor) % divisor;
        }
    }
}
