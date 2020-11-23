using System;
using UnityEngine;

namespace Prototype.Movement
{
    public class ZoneTime : MonoBehaviour
    {
        private float timeOfLastTick;
        private float elapsedTime;

        public event Action Tick;

        public uint CurrentTick { get; private set; }

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

        public float TimeSinceLastTick
        {
            get
            {
                return Time.time - timeOfLastTick;
            }
        }

        private void Update()
        {
            elapsedTime += Time.deltaTime;

            while (elapsedTime > TimePerTick)
            {
                elapsedTime -= TimePerTick;

                OnTick();
            }
        }

        private void OnTick()
        {
            CurrentTick++;
            timeOfLastTick = Time.time;

            Tick?.Invoke();
        }
    }
}
