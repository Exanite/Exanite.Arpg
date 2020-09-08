﻿using Exanite.Arpg.Networking.Client;
using LiteNetLib;
using Prototype.Networking.Players;
using Prototype.Networking.Players.Packets;
using Prototype.Networking.Zones;
using UnityEngine;

namespace Prototype.Networking.Client
{
    public class PlayerController : MonoBehaviour
    {
        public bool useAI;
        public float seed;

        private PlayerInputPacket inputPacket = new PlayerInputPacket();

        public UnityClient client; // inject this later
        public Player player;
        public Zone zone;
        private PlayerMovementBehaviour movementBehaviour;

        private void Start()
        {
            seed = Random.Range(-1000f, 1000f);

            movementBehaviour = GetComponent<PlayerMovementBehaviour>();
        }

        private void FixedUpdate()
        {
            movementBehaviour.input = new PlayerInputData()
            {
                movement = useAI ? GetPerlinMovementInput() : GetMovementInput(),
                tick = zone.Tick,
            };

            SendInput();
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

        public void SendInput()
        {
            inputPacket.data = movementBehaviour.input;

            client.SendPacketToServer(inputPacket, DeliveryMethod.Unreliable);
        }
    }
}
