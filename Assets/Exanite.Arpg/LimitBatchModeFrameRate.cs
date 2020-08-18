using UnityEngine;

namespace Exanite.Arpg
{
    /// <summary>
    /// Used to limit the <see cref="Application.targetFrameRate"/> when the game is running in -batchmode
    /// </summary>
    public class LimitBatchModeFrameRate : MonoBehaviour
    {
        [SerializeField] private int targetFrameRate = 30;

        /// <summary>
        /// Target frame rate when the game is running -batchmode
        /// </summary>
        public int TargetFps
        {
            get
            {
                return targetFrameRate;
            }

            set
            {
                targetFrameRate = value;
            }
        }

        private void Start()
        {
            if (Application.isBatchMode)
            {
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = TargetFps;
            }
        }
    } 
}
