using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CameraTrajectoryRecorder
{
    public class TrackRecorder : MonoBehaviour, IPointerMoveHandler, IPointerDownHandler, IPointerUpHandler
    {
        public Transform CameraRotationContainer;
        public bool IsPointerDown;
        public float RotationSensitivity;

        public List<CameraTrackKey> SourceRotations = new List<CameraTrackKey>();

        private CameraTrack _cameraTrack;
        private float _recordingStartedAt;

        public bool InProcess { get; private set; }
        public CameraTrack StartRecording(Camera camera, CameraTrack cameraTrack)
        {
            if (camera == null)
            {
                Debug.LogError("Incorrect data.");
                return cameraTrack;
            }

            if (cameraTrack == null)
            {
                cameraTrack = ScriptableObject.CreateInstance<CameraTrack>();
            }

            InProcess = true;
            _recordingStartedAt = Time.timeSinceLevelLoad;
            SourceRotations.Clear();
            _cameraTrack = cameraTrack;
            AddKey();

            return cameraTrack;
        }

        public void StopProcess()
        {
            InProcess = false;
            _cameraTrack.SetKeys(SourceRotations);
        }

        private void AddKey()
        {
            SourceRotations.Add(new CameraTrackKey
            {
                Rotation = CameraRotationContainer.rotation,
                TimeStamp = Time.timeSinceLevelLoad - _recordingStartedAt
            });
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
                
                CameraRotationContainer.rotation = Quaternion.Euler(eulerAngles);
                AddKey();
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