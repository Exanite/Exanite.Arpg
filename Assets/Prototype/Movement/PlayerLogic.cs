using Prototype.Networking.Players.Data;
using UnityEngine;

namespace Prototype.Movement
{
    public class PlayerLogic
    {
        public float mapSize = 10;

        public PlayerLogic(float mapSize)
        {
            this.mapSize = mapSize;
        }

        public PlayerStateData Simulate(PlayerStateData stateData, PlayerInputData inputData)
        {
            float timePerTick = Time.fixedDeltaTime;

            stateData.position += (Vector3)inputData.movement * timePerTick * 5;
            stateData.position = Wrap(stateData.position);

            return stateData;
        }

        private Vector2 Wrap(Vector2 position)
        {
            position.x = Wrap(position.x, -mapSize, mapSize);
            position.y = Wrap(position.y, -mapSize, mapSize);

            return position;
        }

        private float Wrap(float value, float min, float max)
        {
            return Modulo(value - min, max - min) + min;
        }

        private float Modulo(float value, float divisor)
        {
            return ((value % divisor) + divisor) % divisor;
        }
    }
}
