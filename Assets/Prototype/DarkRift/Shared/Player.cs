using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prototype.DarkRift.Shared
{
    public class Player
    {
        public ushort id;
        public Transform transform;

        public Vector2 movementInput;

        public Player(ushort id, Scene scene)
        {
            this.id = id;

            transform = new GameObject($"Player {id.ToString()}").transform;
            SceneManager.MoveGameObjectToScene(transform.gameObject, scene);
        }
    } 
}
