using Exanite.Arpg;
using UnityEngine;

namespace Prototype.Movement
{
    public class PlayerCharacter : MonoBehaviour
    {
        public ZoneTime Time;
        public float mapSize = 10;

        public PlayerStateData currentStateData;

        public event EventHandler<PlayerCharacter, PlayerStateData> StateUpdated;

        protected virtual void OnEnable()
        {
            Time.Tick += OnTick;
        }

        protected virtual void OnDisable()
        {
            Time.Tick -= OnTick;
        }

        public virtual void ApplyState(PlayerStateData data)
        {
            currentStateData = data;

            transform.position = data.position;
        }

        protected virtual void OnTick() { }

        protected void OnStateUpdated()
        {
            StateUpdated?.Invoke(this, currentStateData);
        }
    }
}
