using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

namespace CameraTrajectoryRecorder
{
    public class CameraTrack : ScriptableObject
    {
        public List<BezierKnot> Knots = new List<BezierKnot>();

        public bool IsEmpty()
        {
            return Knots.Count == 0;
        }
    }
}