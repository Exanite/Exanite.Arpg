using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prototype.LiteNetLib.Players
{
    public class Player
    {
        private PlayerConnection connection;

        public PlayerCharacter character;

        public Vector2 movementInput; // todo better impl

        public Player(PlayerConnection connection, Scene scene)
        {
            Connection = connection;

            CreatePlayerCharacter(scene); // todo should not be automatically called later on
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

        public void CreatePlayerCharacter(Scene scene)
        {
            character = new GameObject($"Player {Id}").AddComponent<PlayerCharacter>();
            character.player = this;

            SceneManager.MoveGameObjectToScene(character.gameObject, scene);
        }
    }
}
