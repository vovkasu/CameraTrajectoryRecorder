using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

namespace CameraTrajectoryRecorder
{
    public class CameraTrack : ScriptableObject
    {
        public List<BezierKnot> Knots = new List<BezierKnot>();
        public string TrackId;

        public bool IsEmpty()
        {
            return Knots.Count == 0;
        }

        public void SetKnots(IEnumerable<BezierKnot> knots)
        {
            Knots = knots.Select(_=>_).ToList();
        }
    }
}