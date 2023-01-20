using UnityEngine;

namespace CameraTrajectoryRecorder
{
    public class CameraLookAtObject : MonoBehaviour
    {
        public Transform Target;

        private void LateUpdate()
        {
            transform.LookAt(Target);
        }
    }
}