using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CameraTrajectoryRecorder
{
    public class GameController : MonoBehaviour
    {
        public CameraTrack CameraTrack;
        public Camera MainCamera;
        public TrackRecorder TrackRecorder;
        public TrackPlayer TrackPlayer;
        public TrackStorage TrackStorage;

        [Header("UI")] 
        public Button StopButton;
        public TextMeshProUGUI StopButtonText;
        public Button PlayButton;
        public Button SaveTrackButton;
        public Transform MainUi;

        private void Start()
        {
            TrackPlayer.OnCompleted -= UpdateUi;
            TrackPlayer.OnCompleted += UpdateUi;

            if (CameraTrack == null)
            {
                CameraTrack = TrackStorage.GetSelectedTrack();
            }
            UpdateUi();
        }

        public void StartTrackRecording()
        {
            StopProcessInternal();

            CameraTrack = TrackRecorder.StartRecording(MainCamera, CameraTrack);
            UpdateUi();
        }

        public void PlayTrack()
        {
            StopProcessInternal();
            TrackPlayer.Play(CameraTrack);
            UpdateUi();
        }

        public void StopProcess()
        {
            StopProcessInternal();
            UpdateUi();
        }

        public void SaveTrack()
        {
            TrackStorage.SaveTrack(CameraTrack);
        }

        public void LoadTrackListScene()
        {
            SceneManager.LoadScene("TrackListScene", LoadSceneMode.Single);
        }

        private void StopProcessInternal()
        {
            if (TrackPlayer.InProcess)
            {
                TrackPlayer.StopProcess();
            }

            if (TrackRecorder.InProcess)
            {
                TrackRecorder.StopProcess();
            }
        }

        private void UpdateUi()
        {
            var inProcess = TrackPlayer.InProcess || TrackRecorder.InProcess;
            StopButton.gameObject.SetActive(inProcess);
            if (inProcess)
            {
                StopButtonText.text = TrackPlayer.InProcess ? "Stop player" : "Stop recording";
            }

            MainUi.gameObject.SetActive(!inProcess);
            SaveTrackButton.interactable = 
                PlayButton.interactable = 
                    CameraTrack != null && !CameraTrack.IsEmpty();
        }
    }
}