using System;
using UnityEngine;

namespace CameraTrajectoryRecorder
{
    [Serializable]
    public class CameraTrackKey
    {
        public Quaternion Rotation;
        public float TimeStamp;
    }
}