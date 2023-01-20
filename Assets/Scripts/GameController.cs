using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CameraTrajectoryRecorder
{
    public class GameController : MonoBehaviour
    {
        public CameraTrack CameraTrack;
        public Camera MainCamera;
        public TrackRecorder TrackRecorder;
        public TrackPlayer TrackPlayer;

        [Header("UI")] 
        public Button StopButton;
        public TextMeshProUGUI StopButtonText;
        public Button PlayButton;
        public Transform MainUi;

        private void Start()
        {
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
        }

        public void StopProcess()
        {
            StopProcessInternal();
            UpdateUi();
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
            PlayButton.interactable = CameraTrack != null && !CameraTrack.IsEmpty();
        }
    }
}