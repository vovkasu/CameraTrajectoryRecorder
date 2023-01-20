using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CameraTrajectoryRecorder
{
    public class TrackListController : MonoBehaviour
    {
        public TrackInfo TrackInfoPrefab;
        public Transform TrackInfoContainer;
        public TrackStorage TrackStorage;

        public Text State;
        public Text PreviewButtonText;

        private void Start()
        {
            var allTrackIds = TrackStorage.GetAllTrackIds();
            foreach (var trackId in allTrackIds.List)
            {
                var trackInfo = Instantiate(TrackInfoPrefab, TrackInfoContainer);
                trackInfo.Init(trackId);
                trackInfo.OnDelete += DeleteTrack;
                trackInfo.OnSelect += SelectTrack;
            }

            State.text = allTrackIds.List.Count == 0 ? "Empty list." : "Select track";
            TrackStorage.ResetSelectedTrack();
            UpdatePreviewButton();
        }

        private void SelectTrack(TrackInfo obj)
        {
            TrackStorage.SelectTrack(obj.TrackId);
            UpdatePreviewButton();
            State.text = $"Track '{obj.TrackId}' loaded";
        }

        private void DeleteTrack(TrackInfo obj)
        {
            TrackStorage.DeleteTrack(obj.TrackId);
            Destroy(obj.gameObject);
            TrackStorage.ResetSelectedTrack();
            UpdatePreviewButton();
            State.text = $"Track {obj.TrackId} deleted.";
        }

        public void LoadMainScene()
        {
            SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
        }

        private void UpdatePreviewButton()
        {
            PreviewButtonText.text = TrackStorage.GetSelectedTrack() == null ? "Create track" : "Play Track";
        }
    }
}