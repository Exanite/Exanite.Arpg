using Exanite.Arpg;
using Prototype.LiteNetLib.Zones;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prototype.LiteNetLib.Players
{
    public class Player
    {
        private PlayerConnection connection;

        public PlayerCharacter character;

        public Vector2 movementInput; // todo better impl

        public Player(PlayerConnection connection)
        {
            Connection = connection;
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

        public void CreatePlayerCharacter(Zone zone)
        {
            character = zone.scene.InstantiateNew($"Player {Id}").AddComponent<PlayerCharacter>();
            character.player = this;
        }
    }
}
