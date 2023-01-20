using UnityEngine;

namespace CameraTrajectoryRecorder
{
    public class TrackRecorder : MonoBehaviour
    {
        public bool InProcess { get; private set; }
        public CameraTrack StartRecording(Camera mainCamera, CameraTrack cameraTrack)
        {
            if (mainCamera == null)
            {
                Debug.LogError("Incorrect data.");
                return cameraTrack;
            }

            if (cameraTrack == null)
            {
                cameraTrack = ScriptableObject.CreateInstance<CameraTrack>();
            }

            InProcess = true;

            return cameraTrack;
        }

        public void StopProcess()
        {
            InProcess = false;
        }
    }
}