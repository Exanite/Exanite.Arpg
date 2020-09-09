﻿using Prototype.Networking.Players.Data;
using Prototype.Networking.Zones;
using UnityEngine;
using Zenject;

namespace Prototype.Networking.Players
{
    public class PlayerMovementBehaviour : MonoBehaviour
    {
        public const float MapSize = 10; // ! temp

        public PlayerInputData input;

        private Zone zone;
        private PlayerCharacter character;

        [Inject]
        public void Inject(Player player, Zone zone)
        {
            if (!player.IsLocal && !player.IsServer)
            {
                enabled = false;
            }

            this.zone = zone;

            character = GetComponent<PlayerCharacter>();
        }

        private void FixedUpdate()
        {
            var currentData = character.interpolation.current;
            var newData = Simulate(currentData, input);

            character.interpolation.UpdateData(newData);
        }

        public PlayerUpdateData Simulate(PlayerUpdateData updateData, PlayerInputData inputData)
        {
            updateData.playerPosition += (Vector3)input.movement * zone.TimePerTick * 5;
            updateData.playerPosition = Wrap(updateData.playerPosition);

            updateData.tick++;

            return updateData;
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
