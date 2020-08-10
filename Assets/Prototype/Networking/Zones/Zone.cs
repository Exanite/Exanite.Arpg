using System;
using System.Collections.Generic;
using Exanite.Arpg;
using Prototype.Networking.Players;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prototype.Networking.Zones
{
    public class Zone
    {
        private static int counter = 0; // ! hack to prevent scenes from having the same name

        public Guid guid;

        public Transform root;
        public Scene scene;

        public HashSet<Player> players = new HashSet<Player>();

        public Zone(GameObject zonePrefab) : this(Guid.NewGuid(), zonePrefab) { }

        public Zone(Guid guid, GameObject zonePrefab)
        {
            this.guid = guid;

            CreateZone(zonePrefab);
        }

        public event EventHandler<Zone, Player> PlayerEnteredEvent;

        public event EventHandler<Zone, Player> PlayerLeftEvent;

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

        private void CreateZone(GameObject levelPrefab)
        {
            var createSceneParameters = new CreateSceneParameters(LocalPhysicsMode.Physics3D);
            scene = SceneManager.CreateScene($"{guid.ToString()} ({counter})", createSceneParameters);

            root = scene.Instantiate(levelPrefab).transform;

            counter++;
        }
    }
}
