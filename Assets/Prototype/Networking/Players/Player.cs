﻿using Exanite.Arpg;
using Prototype.Networking.Zones;
using UnityEngine;

namespace Prototype.Networking.Players
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