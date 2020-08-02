using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prototype.LiteNetLib.Shared
{
    public class Player
    {
        private PlayerConnection connection;

        public Transform transform; // todo Replace with reference to PlayerCharacter class
        public Vector2 movementInput; // todo better impl

        public Player(PlayerConnection connection, Scene scene)
        {
            Connection = connection;

            CreatePlayerCharacter(Id, scene); // todo should not be automatically called later on
        }

        public void CreatePlayerCharacter(int id, Scene scene)
        {
            transform = new GameObject($"Player {id.ToString()}").transform;
            SceneManager.MoveGameObjectToScene(transform.gameObject, scene);
        }

        public PlayerConnection Connection
        {
            get
            {
                return connection;
            }

            set
            {
                connection = value;
            }
        }

        public int Id
        {
            get
            {
                return connection.Id;
            }
        }
    }
}
