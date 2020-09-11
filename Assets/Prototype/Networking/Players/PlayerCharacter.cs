using System.Collections.Generic;
using Exanite.Arpg.Networking.Client;
using Prototype.Networking.Client;
using Prototype.Networking.Players.Data;
using Prototype.Networking.Startup;
using Prototype.Networking.Zones;
using UnityEngine;
using Zenject;

namespace Prototype.Networking.Players
{
    public class PlayerCharacter : MonoBehaviour
    {
        private Queue<ReconciliationData> unconfirmedTicks = new Queue<ReconciliationData>();
        private uint lastTickFromServer;

        private PlayerInputData input;
        private bool reconciliationActive = false;

        private PlayerController controller;
        private PlayerInterpolation interpolation;
        private PlayerLogic logic;

        private UnityClient client;
        private Player player;
        private Zone zone;
        private GameStartSettings settings;

        [Inject]
        public void Inject([InjectOptional] UnityClient client, Player player, Zone zone, GameStartSettings settings)
        {
            this.client = client;
            Player = player;
            Zone = zone;
            this.settings = settings;

            Controller = GetComponent<PlayerController>();
            Interpolation = GetComponent<PlayerInterpolation>();
            Logic = GetComponent<PlayerLogic>();
        }

        public PlayerController Controller
        {
            get
            {
                return controller;
            }

            private set
            {
                controller = value;
            }
        }

        public PlayerInterpolation Interpolation
        {
            get
            {
                return interpolation;
            }

            private set
            {
                interpolation = value;
            }
        }

        public PlayerLogic Logic
        {
            get
            {
                return logic;
            }

            private set
            {
                logic = value;
            }
        }

        public Player Player
        {
            get
            {
                return player;
            }

            private set
            {
                player = value;
            }
        }

        public Zone Zone
        {
            get
            {
                return zone;
            }

            private set
            {
                zone = value;
            }
        }

        private void FixedUpdate()
        {
            if (player.IsLocal)
            {
                input = Controller.GetInput();

                Controller.SendInput(input);
            }

            if (player.IsServer || player.IsLocal)
            {
                var currentData = Interpolation.current;
                var newData = Logic.Simulate(currentData, input);

                Interpolation.UpdateData(newData);
            }

            if (player.IsLocal)
            {
                var reconciliationData = new ReconciliationData(zone.Tick, Interpolation.current, input);

                unconfirmedTicks.Enqueue(reconciliationData);
            }

            reconciliationActive = false;
        }

        private void OnGUI() // ! debug
        {
            if (player.IsLocal && (!settings.useAI || player.Id == 0))
            {
                GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
                {
                    GUILayout.FlexibleSpace();

                    GUILayout.Label($"Client RTT: {client.Server.Ping * 2}");
                    GUILayout.Label($"Zone tick: {Zone.Tick}");
                    GUILayout.Label($"Last tick from server: {lastTickFromServer}");
                    GUILayout.Label($"Unconfirmed Ticks: {unconfirmedTicks.Count}");
                    GUILayout.Label($"Reconciliation active: {reconciliationActive}");
                }
                GUILayout.EndArea();
            }
        }

        public void DrawWithGL(Material material, Color color, float size = 0.25f) // ! temp
        {
            material.SetPass(0);
            GL.Begin(GL.TRIANGLES);
            {
                GL.Color(color);

                Vector3 position = Player.Character.transform.position;

                GL.Vertex3(position.x - size, position.y - size, transform.position.z);
                GL.Vertex3(position.x - size, position.y + size, transform.position.z);
                GL.Vertex3(position.x + size, position.y + size, transform.position.z);

                GL.Vertex3(position.x + size, position.y + size, transform.position.z);
                GL.Vertex3(position.x + size, position.y - size, transform.position.z);
                GL.Vertex3(position.x - size, position.y - size, transform.position.z);
            }
            GL.End();
        }

        // server only
        public void OnInput(PlayerInputData inputData)
        {
            input = inputData;
        }

        // client only
        public void OnUpdate(PlayerUpdateData updateData, uint tick)
        {
            if (tick < lastTickFromServer)
            {
                return;
            }

            lastTickFromServer = tick;

            if (player.IsLocal)
            {
                while (unconfirmedTicks.Count > 0 && unconfirmedTicks.Peek().tick < lastTickFromServer)
                {
                    unconfirmedTicks.Dequeue();
                }

                if (unconfirmedTicks.Count > 0 && unconfirmedTicks.Peek().tick == lastTickFromServer)
                {
                    reconciliationActive = true;

                    ReconciliationData reconciliationData = unconfirmedTicks.Dequeue();

                    if (Vector3.Distance(reconciliationData.updateData.playerPosition, updateData.playerPosition) > 0.05f)
                    {
                        var ticksToProcess = unconfirmedTicks.ToArray();
                        Interpolation.current = updateData;

                        for (int i = 0; i < ticksToProcess.Length; i++)
                        {
                            PlayerUpdateData newUpdateData = Logic.Simulate(Interpolation.current, ticksToProcess[i].inputData);
                            Interpolation.UpdateData(newUpdateData);
                        }
                    }
                }

                zone.Tick = tick + (uint)(client.Server.Ping * zone.TimePerTick);
            }
            else
            {
                Interpolation.UpdateData(updateData);
            }
        }
    }
}
