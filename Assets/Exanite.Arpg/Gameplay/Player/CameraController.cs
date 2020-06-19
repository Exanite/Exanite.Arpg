using UnityEngine;

namespace Exanite.Arpg.Gameplay.Player
{
    public class CameraController : MonoBehaviour
    {
        public Transform target;
        public float rotation = 45;
        public float angle = 75;
        public float distance = 20;

        private void LateUpdate()
        {
            if (!target)
            {
                return;
            }

            transform.rotation = Quaternion.Euler(angle, rotation, 0);

            transform.position = target.position - transform.forward * distance;
        }
    } 
}
