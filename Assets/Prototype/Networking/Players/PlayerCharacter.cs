using Prototype.Networking.Zones;
using UnityEngine;

namespace Prototype.Networking.Players
{
    public class PlayerCharacter : MonoBehaviour
    {
        public const float maxInterpolationDistance = 2; // ! temp

        public Player player;
        public Zone zone;

        public Vector3 currentPosition;
        public Vector3 previousPosition;
        public int lastUpdateTick;

        private void Update()
        {
            int ticksSinceLastUpdate = zone.Tick - lastUpdateTick;

            float timeSinceLastUpdate = ticksSinceLastUpdate * zone.TimePerTick + zone.TimeSinceLastTick;
            float t = timeSinceLastUpdate / zone.TimePerTick;

            transform.position = Vector3.LerpUnclamped(previousPosition, currentPosition, t);
        }

        public void UpdatePosition(Vector3 newPosition, int tick)
        {
            if ((newPosition - currentPosition).sqrMagnitude < maxInterpolationDistance * maxInterpolationDistance)
            {
                previousPosition = currentPosition;
            }
            else
            {
                previousPosition = newPosition;
            }

            currentPosition = newPosition;

            lastUpdateTick = tick;
        }

        public void DrawWithGL(Material material, Color color, float size = 0.25f) // ! temp
        {
            material.SetPass(0);
            GL.Begin(GL.TRIANGLES);
            {
                GL.Color(color);

                Vector3 position = player.Character.transform.position;

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
