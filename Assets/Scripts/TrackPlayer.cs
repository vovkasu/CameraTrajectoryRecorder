using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

namespace CameraTrajectoryRecorder
{
    public class TrackPlayer : MonoBehaviour
    {
        public Transform CameraPositionContainer;
        public SplineContainer SplineContainer;
        [Range(1f, 10f)]
        public float ClipDuration = 3f;
        private SplineAnimate _splineAnimate;
        public bool InProcess { get; private set; }
        public void Play(CameraTrack cameraTrack)
        {
            if (cameraTrack == null || cameraTrack.IsEmpty())
            {
                Debug.LogError("Incorrect track");
                return;
            }
            InProcess = true;
            SplineContainer.Spline.Knots = cameraTrack.Knots;
            _splineAnimate = CameraPositionContainer.gameObject.AddComponent<SplineAnimate>();
            _splineAnimate.Container = SplineContainer;
            _splineAnimate.Duration = ClipDuration;
            _splineAnimate.Alignment = SplineAnimate.AlignmentMode.World;
            _splineAnimate.Easing = SplineAnimate.EasingMode.None;
            _splineAnimate.Loop = SplineAnimate.LoopMode.Once;
            _splineAnimate.PlayOnAwake = false;
            StartCoroutine(DelayStartAnimation());
        }

        private IEnumerator DelayStartAnimation()
        {
            yield return new WaitForEndOfFrame();
            _splineAnimate.Play();
        }

        public void StopProcess()
        {
            InProcess = false;
            DestroySplineAnimation();
        }

        private void DestroySplineAnimation()
        {
            if (_splineAnimate != null)
            {
                _splineAnimate.Restart(false);
                Destroy(_splineAnimate);
                _splineAnimate = null;
            }
        }
    }
}