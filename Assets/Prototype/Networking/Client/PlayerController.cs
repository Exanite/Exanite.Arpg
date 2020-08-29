using Exanite.Arpg.Networking.Client;
using LiteNetLib;
using Prototype.Networking.Players;
using Prototype.Networking.Players.Packets;
using UnityEngine;

namespace Prototype.Networking.Client
{
    public class PlayerController : MonoBehaviour
    {
        public UnityClient client; // inject this later
        public Player player;

        public bool useAI;

        private float seed;

        private void Start()
        {
            seed = Random.Range(-1000f, 1000f);
        }

        private void FixedUpdate()
        {
            Vector2 input;

            if (useAI)
            {
                input = GetPerlinMovementInput();
            }
            else
            {
                input = GetMovementInput();
            }

            SendMovementInput(input);
        }

        public Vector2 GetMovementInput()
        {
            Vector2 input = Vector2.zero;

            input.x += Input.GetKey(KeyCode.D) ? 1 : 0;
            input.x -= Input.GetKey(KeyCode.A) ? 1 : 0;

            input.y += Input.GetKey(KeyCode.W) ? 1 : 0;
            input.y -= Input.GetKey(KeyCode.S) ? 1 : 0;

            return input.normalized;
        }

        public Vector2 GetPerlinMovementInput()
        {
            float angle = Mathf.PerlinNoise(Time.time * 0.1f + seed, -Time.time * 0.1f + seed) * 360;

            Vector2 input = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            return input * Mathf.PerlinNoise(Time.time * 0.1f + seed, 0) * 2;
        }

        public void SendMovementInput(Vector2 movementInput)
        {
            client.SendPacketToServer(new PlayerInputPacket() { movementInput = movementInput }, DeliveryMethod.Unreliable);
        }
    }
}
