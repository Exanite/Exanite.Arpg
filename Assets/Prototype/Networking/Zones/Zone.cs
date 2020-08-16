using System;
using System.Collections.Generic;
using Exanite.Arpg;
using Prototype.Networking.Players;
using UnityEngine.SceneManagement;

namespace Prototype.Networking.Zones
{
    public class Zone
    {
        public Guid guid;
        public Scene scene;

        public Dictionary<int, Player> playersById = new Dictionary<int, Player>();

        public Zone(string zoneSceneName) : this(Guid.NewGuid(), zoneSceneName) { }

        public Zone(Guid guid, string zoneSceneName)
        {
            this.guid = guid;

            CreateZone(zoneSceneName);
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

        private void CreateZone(string zoneSceneName)
        {
            var loadSceneParameters = new LoadSceneParameters(LoadSceneMode.Additive, LocalPhysicsMode.Physics3D);

            scene = SceneManager.LoadScene("Zone", loadSceneParameters);
        }
    }
}
