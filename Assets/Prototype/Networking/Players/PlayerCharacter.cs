using UnityEngine;

namespace Prototype.Networking.Players
{
    public class PlayerCharacter : MonoBehaviour
    {
        public Player player;

        public Vector3 currentPosition;
        public Vector3 previousPosition;
        public float lastUpdateTime;

        private void Update()
        {
            float timeSinceLastUpdate = Time.time - lastUpdateTime;
            float t = timeSinceLastUpdate / Time.fixedDeltaTime;

            transform.position = Vector3.LerpUnclamped(previousPosition, currentPosition, t);
        }

        public void UpdatePosition(Vector3 newPosition, float time)
        {
            previousPosition = currentPosition;
            currentPosition = newPosition;

            lastUpdateTime = time;
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
