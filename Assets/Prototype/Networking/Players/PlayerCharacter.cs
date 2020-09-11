using Prototype.Networking.Client;
using Prototype.Networking.Players.Data;
using Prototype.Networking.Zones;
using UnityEngine;
using Zenject;

namespace Prototype.Networking.Players
{
    public class PlayerCharacter : MonoBehaviour
    {
        public PlayerInputData input;

        private PlayerController controller;
        private PlayerInterpolation interpolation;
        private PlayerLogic logic;

        private Player player;
        private Zone zone;

        [Inject]
        public void Inject(Player player, Zone zone)
        {
            Player = player;
            Zone = zone;

            Controller = GetComponent<PlayerController>();
            Interpolation = GetComponent<PlayerInterpolation>();
            Logic = GetComponent<PlayerLogic>();
        }

        public PlayerController Controller
        {
            get
            {
                return controller;
            }

            private set
            {
                controller = value;
            }
        }

        public PlayerInterpolation Interpolation
        {
            get
            {
                return interpolation;
            }

            private set
            {
                interpolation = value;
            }
        }

        public PlayerLogic Logic
        {
            get
            {
                return logic;
            }

            private set
            {
                logic = value;
            }
        }

        public Player Player
        {
            get
            {
                return player;
            }

            private set
            {
                player = value;
            }
        }

        public Zone Zone
        {
            get
            {
                return zone;
            }

            private set
            {
                zone = value;
            }
        }

        private void FixedUpdate()
        {
            var currentData = Interpolation.current;
            var newData = Logic.Simulate(currentData, input);

            Interpolation.UpdateData(newData);
        }

        public void DrawWithGL(Material material, Color color, float size = 0.25f) // ! temp
        {
            material.SetPass(0);
            GL.Begin(GL.TRIANGLES);
            {
                GL.Color(color);

                Vector3 position = Player.Character.transform.position;

                GL.Vertex3(position.x - size, position.y - size, transform.position.z);
                GL.Vertex3(position.x - size, position.y + size, transform.position.z);
                GL.Vertex3(position.x + size, position.y + size, transform.position.z);

                GL.Vertex3(position.x + size, position.y + size, transform.position.z);
                GL.Vertex3(position.x + size, position.y - size, transform.position.z);
                GL.Vertex3(position.x - size, position.y - size, transform.position.z);
            }
            GL.End();
        }
    }
}
