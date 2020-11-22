using Prototype.Networking.Players.Data;
using UnityEngine;

namespace Prototype.Movement
{
    public class PlayerCharacter : MonoBehaviour
    {
        public uint tick;
        public float mapSize = 10;

        public PlayerUpdateData currentUpdateData;
    }
}
