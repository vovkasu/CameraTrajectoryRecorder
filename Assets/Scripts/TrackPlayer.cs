using UnityEngine;

namespace CameraTrajectoryRecorder
{
    public class TrackPlayer : MonoBehaviour
    {
        public bool InProcess { get; private set; }
        public void Play(Camera mainCamera, CameraTrack cameraTrack)
        {
            InProcess = true;
        }

        public void StopProcess()
        {
            InProcess = false;
        }
    }
}