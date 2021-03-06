﻿using Prototype.Networking.Players.Data;
using Prototype.Networking.Zones;
using UnityEngine;
using Zenject;

namespace Prototype.Networking.Players
{
    public class PlayerLogic : MonoBehaviour
    {
        public const float MapSize = 10; // ! temp

        private Zone zone;

        [Inject]
        public void Inject(Zone zone)
        {
            this.zone = zone;
        }

        public PlayerUpdateData Simulate(PlayerUpdateData updateData, PlayerInputData inputData)
        {
            updateData.position += (Vector3)inputData.movement * zone.TimePerTick * 5;
            updateData.position = Wrap(updateData.position);

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
