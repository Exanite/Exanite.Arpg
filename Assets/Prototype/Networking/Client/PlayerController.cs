using Exanite.Arpg.Networking.Client;
using LiteNetLib;
using Prototype.Networking.Players.Data;
using Prototype.Networking.Players.Packets;
using Prototype.Networking.Startup;
using Prototype.Networking.Zones;
using UnityEngine;
using Zenject;

namespace Prototype.Networking.Client
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private bool useAI;

        private PlayerInputPacket inputPacket = new PlayerInputPacket();
        private float seed;

        private UnityClient client;
        private Zone zone;

        [Inject]
        public void Inject([InjectOptional] UnityClient client, Zone zone, GameStartSettings settings)
        {
            this.client = client;
            this.zone = zone;

            useAI = settings.useAI;

            seed = Random.Range(-1000f, 1000f);
        }

        public PlayerInputData GetInput()
        {
            var input = new PlayerInputData();
            input.movement = useAI ? GetPerlinMovementInput() : GetMovementInput();

            return input;
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

        public void SendInput(PlayerInputData inputData)
        {
            inputPacket.tick = zone.Tick;
            inputPacket.data = inputData;

            client.SendPacketToServer(inputPacket, DeliveryMethod.Unreliable);
        }
    }
}
