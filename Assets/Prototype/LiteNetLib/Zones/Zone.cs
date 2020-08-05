using System;
using System.Collections.Generic;
using Exanite.Arpg;
using Prototype.LiteNetLib.Players;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prototype.LiteNetLib.Zones
{
    public class Zone
    {
        public Guid guid;

        public Transform root;
        public Scene scene;

        public HashSet<Player> players = new HashSet<Player>();

        public Zone(GameObject zonePrefab)
        {
            guid = Guid.NewGuid();

            CreateZone(zonePrefab);
        }

        public event EventHandler<Zone, Player> PlayerEnteredEvent;

        public event EventHandler<Zone, Player> PlayerLeftEvent;

        private void CreateZone(GameObject levelPrefab)
        {
            var createSceneParameters = new CreateSceneParameters(LocalPhysicsMode.Physics3D);
            scene = SceneManager.CreateScene(guid.ToString(), createSceneParameters);

            root = scene.Instantiate(levelPrefab).transform;
        }

        public void AddPlayer(Player player)
        {
            if (players.Add(player))
            {
                PlayerEnteredEvent?.Invoke(this, player);
            }
        }

        public void RemovePlayer(Player player)
        {
            if (players.Remove(player))
            {
                PlayerLeftEvent?.Invoke(this, player);
            }
        }
    }
}
