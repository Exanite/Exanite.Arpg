using Exanite.Arpg;
using UnityEngine;

namespace Prototype.Movement
{
    public class PlayerCharacter : MonoBehaviour
    {
        public uint tick;
        public float mapSize = 10;

        public PlayerStateData currentStateData;

        public event EventHandler<PlayerCharacter, PlayerStateData> StateUpdated;

        public virtual void ApplyState(PlayerStateData data)
        {
            currentStateData = data;

            transform.position = data.position;
        }

        protected void OnStateUpdated()
        {
            StateUpdated?.Invoke(this, currentStateData);
        }
    }
}
