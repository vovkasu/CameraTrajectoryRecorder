using System.Collections.Generic;
using UnityEngine;

namespace CameraTrajectoryRecorder
{
    public class CameraTrack : ScriptableObject
    {
        public List<CameraTrackKey> Keys = new List<CameraTrackKey>();
        public string TrackId;

        public bool IsEmpty()
        {
            return Keys.Count == 0;
        }

        public void SetKeys(IEnumerable<CameraTrackKey> keys)
        {
            Keys = new List<CameraTrackKey>(keys);
        }
    }
}