using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Splines;

namespace CameraTrajectoryRecorder
{
    public class TrackRecorder : MonoBehaviour, IPointerMoveHandler, IPointerDownHandler, IPointerUpHandler
    {
        public Transform CameraRotationContainer;
        public Transform PointForRecording;
        public SplineContainer SplineContainer;
        [Range(0.1f, 3f)]
        public float RotationSensitivity = 0.25f;
        [Range(0f, 1f)]
        public float ReducePointsEpsilon = 0.15f;
        [Range(0f, 1f)]
        public float SplineTension = 1 / 4f;
        public bool IsPointerDown;
        public List<Vector3> SourcePositions = new List<Vector3>(1024);

        private Transform _cameraTransform;
        private CameraTrack _cameraTrack;
        
        public bool InProcess { get; private set; }
        public CameraTrack StartRecording(Camera camera, CameraTrack cameraTrack)
        {
            if (camera == null)
            {
                Debug.LogError("Incorrect data.");
                return cameraTrack;
            }

            _cameraTransform = camera.transform;

            if (cameraTrack == null)
            {
                cameraTrack = ScriptableObject.CreateInstance<CameraTrack>();
            }

            InProcess = true;
            SourcePositions.Clear();
            _cameraTrack = cameraTrack;

            return cameraTrack;
        }

        public void StopProcess()
        {
            InProcess = false;
            BuildSpline(SplineContainer.Spline, SourcePositions);
            _cameraTrack.SetKnots(SplineContainer.Spline.Knots);
        }

        private void BuildSpline(Spline spline, List<Vector3> sourcePositions)
        {
            var reducedResult = new List<float3>();
            List<float3> source = sourcePositions.Select(_=>new float3(_.x,_.y,_.z)).ToList();
            SplineUtility.ReducePoints(source, reducedResult, ReducePointsEpsilon);
            spline.Knots = reducedResult.Select(x => new BezierKnot(x));
            var all = new SplineRange(0, spline.Count);
            spline.SetTangentMode(all, TangentMode.AutoSmooth);
            spline.SetAutoSmoothTension(all, SplineTension);
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
                PointForRecording.position = _cameraTransform.position;
                SourcePositions.Add(PointForRecording.localPosition);
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