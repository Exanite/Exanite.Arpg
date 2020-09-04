using UnityEngine;

namespace Prototype.Networking.Players
{
    public class PlayerCharacter : MonoBehaviour
    {
        public Player player;

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
