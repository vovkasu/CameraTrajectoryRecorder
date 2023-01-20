using System;
using System.Collections.Generic;
using UnityEngine;

namespace CameraTrajectoryRecorder
{
    public class TrackStorage : MonoBehaviour
    {
        private static CameraTrack _selectedTrack;
        private const string _trackIdList = "trackIds";
        private const string _trackIdFormat = "trackId_{0}";

        public CameraTrack GetSelectedTrack()
        {
            return _selectedTrack;
        }

        public void SaveTrack(CameraTrack cameraTrack)
        {
            var trackId = cameraTrack.TrackId;
            if (string.IsNullOrWhiteSpace(trackId))
            {
                trackId = GetNewTrackId();
                cameraTrack.TrackId = trackId;
            }

            var json = JsonUtility.ToJson(cameraTrack);
            PlayerPrefs.SetString(trackId, json);

            SaveTrackIdToList(trackId);
            PlayerPrefs.Save();
        }

        private void SaveTrackIdToList(string trackId)
        {
            var allTrackIds = GetAllTrackIds();
            if (!allTrackIds.Contains(trackId))
            {
                allTrackIds.Add(trackId);
            }

            var trackInJson = JsonUtility.ToJson(allTrackIds);
            PlayerPrefs.SetString(_trackIdList, trackInJson);
            PlayerPrefs.Save();
        }

        private string GetNewTrackId()
        {
            var allTrackIds = GetAllTrackIds();
            for (int i = 0; i <= allTrackIds.Count; i++)
            {
                var trackId = string.Format(_trackIdFormat, i);
                if (!allTrackIds.Contains(trackId))
                {
                    return trackId;
                }
            }
            return string.Format(_trackIdFormat, -1);
        }

        public TrackIdList GetAllTrackIds()
        {
            if (!PlayerPrefs.HasKey(_trackIdList))
            {
                PlayerPrefs.SetString(_trackIdList, JsonUtility.ToJson(new TrackIdList()));
                PlayerPrefs.Save();
            }

            var listInJson = PlayerPrefs.GetString(_trackIdList);
            return JsonUtility.FromJson<TrackIdList>(listInJson);
        }

        public void SelectTrack(string trackId)
        {
            var allTrackIds = GetAllTrackIds();
            if (!allTrackIds.Contains(trackId) || !PlayerPrefs.HasKey(trackId))
            {
                Debug.LogError($"Can not load track {trackId}");
                return;
            }

            var trackInJson = PlayerPrefs.GetString(trackId);
            _selectedTrack = JsonUtility.FromJson<CameraTrack>(trackInJson);
        }
    }

    [Serializable]
    public class TrackIdList
    {
        public List<string> List = new List<string>();

        public int Count => List.Count;

        public bool Contains(string trackId)
        {
            return List.Contains(trackId);
        }

        public void Add(string trackId)
        {
            List.Add(trackId);
        }
    }
}