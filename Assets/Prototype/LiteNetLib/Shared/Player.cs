using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prototype.LiteNetLib.Shared
{
    public class Player
    {
        public int id;
        public Transform transform;

        public Vector2 movementInput;

        public Player(int id, Scene scene)
        {
            this.id = id;

            transform = new GameObject($"Player {id.ToString()}").transform;
            SceneManager.MoveGameObjectToScene(transform.gameObject, scene);
        }
    } 
}
