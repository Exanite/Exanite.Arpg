using Prototype.Networking.Players.Data;
using UnityEngine;

namespace Prototype.Movement
{
    public class PlayerInput
    {
        public PlayerInputData Get()
        {
            var input = new PlayerInputData();
            input.movement = GetMovement();

            return input;
        }

        public Vector2 GetMovement()
        {
            Vector2 input = Vector2.zero;

            input.x += Input.GetKey(KeyCode.D) ? 1 : 0;
            input.x -= Input.GetKey(KeyCode.A) ? 1 : 0;

            input.y += Input.GetKey(KeyCode.W) ? 1 : 0;
            input.y -= Input.GetKey(KeyCode.S) ? 1 : 0;

            return input.normalized;
        }
    }
}
