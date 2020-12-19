using System;
using UnityEngine;

namespace Prototype.Movement
{
    public class ZoneTime : MonoBehaviour
    {
        public event Action Tick;

        public uint CurrentTick { get; private set; }

        public float TimeSinceLastTick { get; private set; }

        public float CurrentTime
        {
            get
            {
                return CurrentTick * TimePerTick;
            }
        }

        public float TimePerTick
        {
            get
            {
                return Time.fixedDeltaTime;
            }
        }

        private void Update()
        {
            TimeSinceLastTick += Time.deltaTime;

            while (TimeSinceLastTick > TimePerTick)
            {
                TimeSinceLastTick -= TimePerTick;

                OnTick();
            }
        }

        private void OnTick()
        {
            CurrentTick++;

            Tick?.Invoke();
        }
    }
}
