using System;
using System.Collections.Generic;
using Exanite.Arpg;
using Prototype.Networking.Players;
using UnityEngine.SceneManagement;

namespace Prototype.Networking.Zones
{
    public class Zone
    {
        private static int counter = 0; // ! hack to prevent scenes from having the same name

        public Guid guid;
        public Scene scene;

        public Dictionary<int, Player> playersById = new Dictionary<int, Player>();

        public Zone() : this(Guid.NewGuid()) { }

        public Zone(Guid guid)
        {
            this.guid = guid;

            CreateZone();
        }

        public event EventHandler<Zone, Player> PlayerEnteredEvent;

        public event EventHandler<Zone, Player> PlayerLeftEvent;

        public void AddPlayer(Player player)
        {
            if (!playersById.ContainsKey(player.Id))
            {
                playersById.Add(player.Id, player);
                PlayerEnteredEvent?.Invoke(this, player);
            }
        }

        public void RemovePlayer(Player player)
        {
            if (playersById.Remove(player.Id))
            {
                PlayerLeftEvent?.Invoke(this, player);
            }
        }

        private void CreateZone()
        {
            var createSceneParameters = new CreateSceneParameters(LocalPhysicsMode.Physics3D);
            scene = SceneManager.CreateScene($"{guid.ToString()} ({counter})", createSceneParameters);

            counter++;
        }
    }
}
