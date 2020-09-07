using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Exanite.Arpg;
using Prototype.Networking.Players;
using UnityEngine.SceneManagement;
using Zenject;
using UnityTime = UnityEngine.Time;

namespace Prototype.Networking.Zones
{
    public class Zone
    {
        private readonly Guid guid;
        private readonly bool isServer;

        private bool isCreated;
        private Scene scene;

        private int tick;

        public Dictionary<int, Player> playersById = new Dictionary<int, Player>();

        public Zone(string zoneSceneName, bool isServer) : this(Guid.NewGuid(), zoneSceneName, isServer) { }

        public Zone(Guid guid, string zoneSceneName, bool isServer)
        {
            this.guid = guid;
            this.isServer = isServer;
        }

        public event EventHandler<Zone, Player> PlayerEnteredEvent;

        public event EventHandler<Zone, Player> PlayerLeftEvent;

        public Guid Guid
        {
            get
            {
                return guid;
            }
        }

        public bool IsServer
        {
            get
            {
                return isServer;
            }
        }

        public bool IsCreated
        {
            get
            {
                return isCreated;
            }

            private set
            {
                isCreated = value;
            }
        }

        public Scene Scene
        {
            get
            {
                return scene;
            }

            private set
            {
                scene = value;
            }
        }

        public int Tick
        {
            get
            {
                return tick;
            }

            set
            {
                tick = value;
            }
        }

        public float Time
        {
            get
            {
                return Tick * TimePerTick;
            }
        }

        public float TimePerTick
        {
            get
            {
                return UnityTime.fixedDeltaTime;
            }
        }

        public float TimeSinceLastTick
        {
            get
            {
                return UnityTime.time - UnityTime.fixedTime;
            }
        }

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
            if (IsCreated)
            {
                throw new InvalidOperationException("Zone has already been created.");
            }

            Action<DiContainer> bindings = (container) =>
            {
                container.Bind<Zone>().FromInstance(this).AsSingle();
            };

            Scene = await sceneLoader.LoadAdditiveScene(zoneSceneName, parent, bindings);

            IsCreated = true;
        }

        public async UniTask Destroy(SceneLoader sceneLoader)
        {
            if (!IsCreated)
            {
                return;
            }

            foreach (var player in playersById.Values.ToArray())
            {
                RemovePlayer(player);
            }

            await sceneLoader.UnloadScene(Scene);
        }
    }
}
