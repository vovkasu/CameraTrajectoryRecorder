using UnityEngine;
using UnityEngine.EventSystems;

namespace CameraTrajectoryRecorder
{
    public class TrackRecorder : MonoBehaviour, IPointerMoveHandler, IPointerDownHandler, IPointerUpHandler
    {
        public Transform CameraRotationContainer;
        public float RotationSensitivity = 0.25f;
        public bool IsPointerDown;
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

        #region User manipulations
        public void OnPointerMove(PointerEventData eventData)
        {
            if (IsPointerDown && InProcess)
            {
                var delta = eventData.delta * RotationSensitivity;
                var sourceEulerAngles = CameraRotationContainer.rotation.eulerAngles;
                var eulerAngles = new Vector3(
                    sourceEulerAngles.x - delta.y, 
                    sourceEulerAngles.y + delta.x,
                    sourceEulerAngles.z);
                
                Debug.Log($"OnPointerMove:{delta} eulerAngles:{eulerAngles}");
                CameraRotationContainer.rotation = Quaternion.Euler(eulerAngles);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            IsPointerDown = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            IsPointerDown = false;
        }
        #endregion
    }
}