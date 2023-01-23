using System;
using UnityEngine;

namespace CameraTrajectoryRecorder
{
    public class TrackPlayer : MonoBehaviour
    {
        public Transform CameraRotationContainer;
        public TimeMode TimeMode = TimeMode.FixedUpdate;
        public event Action OnCompleted;
        private CameraRotationAnimation _cameraRotationAnimation;

        public bool InProcess { get; private set; }
        public void Play(CameraTrack cameraTrack)
        {
            if (cameraTrack == null || cameraTrack.IsEmpty())
            {
                Debug.LogError("Incorrect track");
                return;
            }
            InProcess = true;

            _cameraRotationAnimation = CameraRotationContainer.gameObject.AddComponent<CameraRotationAnimation>();
            _cameraRotationAnimation.OnCompleted += OnAnimationCompleted;
            _cameraRotationAnimation.Play(cameraTrack, TimeMode, CameraRotationContainer);
        }


        public void StopProcess()
        {
            _cameraRotationAnimation.OnCompleted -= OnAnimationCompleted;
            Destroy(_cameraRotationAnimation);
            InProcess = false;
        }

        private void OnAnimationCompleted()
        {
            StopProcess();
            if (OnCompleted != null) OnCompleted();
        }
    }

    public enum TimeMode
    {
        Update,
        FixedUpdate
    }
}