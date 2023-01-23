using System;
using System.Linq;
using UnityEngine;

namespace CameraTrajectoryRecorder
{
    public class CameraRotationAnimation : MonoBehaviour
    {
        public event Action OnCompleted; 
        private CameraTrack _cameraTrack;
        private TimeMode _timeMode;
        private float _startedAt;
        private Transform _targetTransform;
        private float _completedAt;
        public int _fromKeyIndex;

        public void Play(CameraTrack cameraTrack, TimeMode timeMode, Transform targetTransform)
        {
            if (cameraTrack == null || cameraTrack.Keys == null || cameraTrack.Keys.Count < 2)
            {
                throw new ArgumentNullException(nameof(cameraTrack));
            }
            if (targetTransform == null)
            {
                throw new ArgumentNullException(nameof(targetTransform));
            }
            _cameraTrack = cameraTrack;
            _timeMode = timeMode;
            _startedAt = Time.timeSinceLevelLoad;
            _targetTransform = targetTransform;
            _completedAt = _startedAt + _cameraTrack.Keys.Last().TimeStamp;

            InitStartPosition();
        }

        private void InitStartPosition()
        {
            _fromKeyIndex = 0;
            SetRotation(_cameraTrack.Keys[0].Rotation);

        }

        private void Update()
        {
            if (_cameraTrack == null || _timeMode != TimeMode.Update) return;
            UpdateAnimation();
        }

        private void FixedUpdate()
        {
            if (_cameraTrack == null || _timeMode != TimeMode.FixedUpdate) return;
            UpdateAnimation();
        }

        private void UpdateAnimation()
        {
            if (Time.timeSinceLevelLoad >= _completedAt)
            {
                CompleteAnimation();
            }

            var time = Time.timeSinceLevelLoad - _startedAt;

            if (_cameraTrack.Keys[_fromKeyIndex + 1].TimeStamp < time)
            {
                for (int i = _fromKeyIndex+1; i < _cameraTrack.Keys.Count; i++)
                {
                    if (_cameraTrack.Keys[i].TimeStamp > time)
                    {
                        _fromKeyIndex = i - 1;
                        break;
                    }
                }
            }

            var timeInSector = time - _cameraTrack.Keys[_fromKeyIndex].TimeStamp;
            var segmentDuration = _cameraTrack.Keys[_fromKeyIndex+1].TimeStamp -
                                 _cameraTrack.Keys[_fromKeyIndex].TimeStamp;
            var inSegmentTime = timeInSector / segmentDuration;
            var rotation = Quaternion.Lerp(
                _cameraTrack.Keys[_fromKeyIndex].Rotation,
                _cameraTrack.Keys[_fromKeyIndex + 1].Rotation,
                inSegmentTime);
            SetRotation(rotation);
        }

        private void CompleteAnimation()
        {
            SetRotation(_cameraTrack.Keys.Last().Rotation);
            if (OnCompleted != null) OnCompleted();
        }

        private void SetRotation(Quaternion rotation)
        {
            _targetTransform.rotation = rotation;
        }
    }
}