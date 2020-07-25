using DarkRift;
using Exanite.Arpg.Networking;
using Exanite.Arpg.Networking.Client;
using Exanite.Arpg.Networking.Shared;
using Prototype.DarkRift.Shared;
using UnityEngine;

namespace Prototype.DarkRift.Client
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Current; //! hack for making sure only 1 player is locally controlled at a time

        public UnityClient client;
        public Player player;

        public bool forceEnableControls = false;

        private float seed;

        private void Start()
        {
            seed = Random.Range(-1000f, 1000f);
        }

        private void Update()
        {
            if (forceEnableControls)
            {
                Current = this;

                forceEnableControls = false;
            }

            //if (Current == this)
            {
                Vector2 input = GetMovementInput();

                if (input != Vector2.zero)
                {
                    SendMovementInput(input);
                }
            }
        }

        public Vector2 GetMovementInput()
        {
            float angle = Mathf.PerlinNoise(Time.time * 0.1f + seed, -Time.time * 0.1f + seed) * 360;

            Vector2 input = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            //input.x += Input.GetKey(KeyCode.D) ? 1 : 0;
            //input.x -= Input.GetKey(KeyCode.A) ? 1 : 0;

            //input.y += Input.GetKey(KeyCode.W) ? 1 : 0;
            //input.y -= Input.GetKey(KeyCode.S) ? 1 : 0;

            return input.normalized;
        }

        public void SendMovementInput(Vector2 movementInput)
        {
            using (var writer = DarkRiftWriter.Create())
            {
                writer.WriteVector2(movementInput);

                using (var message = Message.Create(MessageTag.PlayerInput, writer))
                {
                    client.SendMessage(message, SendMode.Reliable);
                }
            }
        }
    }
}
