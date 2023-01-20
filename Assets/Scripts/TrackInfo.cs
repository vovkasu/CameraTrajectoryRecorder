using System;
using UnityEngine;
using UnityEngine.UI;

namespace CameraTrajectoryRecorder
{
    public class TrackInfo : MonoBehaviour
    {
        public string TrackId;
        public event Action<TrackInfo> OnDelete; 
        public event Action<TrackInfo> OnSelect;
        public Text Description;

        public void Init(string trackId)
        {
            TrackId = trackId;
            Description.text = $"Load track {trackId}";
        }

        public void Delete()
        {
            if (OnDelete != null)
            {
                OnDelete(this);
            }
        }

        public void Select()
        {
            if (OnSelect != null)
            {
                OnSelect(this);
            }
        }
    }
}