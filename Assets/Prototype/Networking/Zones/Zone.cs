using System;
using System.Collections.Generic;
using System.Linq;
using Exanite.Arpg;
using Prototype.Networking.Players;
using UniRx.Async;
using UnityEngine.SceneManagement;

namespace Prototype.Networking.Zones
{
    public class Zone
    {
        public bool isCreated;

        public Guid guid;
        public Scene scene;

        public Dictionary<int, Player> playersById = new Dictionary<int, Player>();

        public Zone(string zoneSceneName) : this(Guid.NewGuid(), zoneSceneName) { }

        public Zone(Guid guid, string zoneSceneName)
        {
            this.guid = guid;
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

        public async UniTask Create(string zoneSceneName, Scene parent, SceneLoader sceneLoader)
        {
            if (isCreated)
            {
                throw new InvalidOperationException("Zone has already been created.");
            }

            scene = await sceneLoader.LoadAdditiveScene(zoneSceneName, parent);

            isCreated = true;
        }

        public async UniTask Destroy(SceneLoader sceneLoader)
        {
            if (!isCreated)
            {
                return;
            }

            foreach (var player in playersById.Values.ToArray())
            {
                RemovePlayer(player);
            }

            await sceneLoader.UnloadScene(scene);
        }
    }
}
