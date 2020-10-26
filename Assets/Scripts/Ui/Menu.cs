using System;
using BreathExercise;
using Data;
using Helpers;
using Input;
using TMPro;
using Tween;
using UnityEngine;
using UnityEngine.UI;

namespace Ui {
    public class Menu : MonoBehaviour {
        private const int NumberSize = 150;

        [SerializeField] private GameObject menuCanvas;
        [SerializeField] private TextMeshProUGUI headerText;
        [SerializeField] private TextMeshProUGUI resultsText;
        [SerializeField] private TextMeshProUGUI menuNewSessionText;
        [SerializeField] public Image menuNewSessionButtonImage;
        [SerializeField] private TextMeshProUGUI menuExitGameText;
        [SerializeField] private Image menuExitGameButtonImage;
        [SerializeField] private TextMeshProUGUI instructionsText;

        private bool _isVisible;
        private float _targetRotation;
        private float _currentRotation;

        private Color ActiveColor() => new Color(1, 1, 1, this._isVisible ? 1f : 0f);


        private void Awake() {
            var clearWhite = new Color(1, 1, 1, 0);

            HeldButtonUi.AddTo(this.menuNewSessionButtonImage, Inputs.PrimaryButton);
            HeldButtonUi.AddTo(this.menuExitGameButtonImage, Inputs.ExitButton);

            FloatTarget.AddTo(this.gameObject,
                v => {
                    this._currentRotation = v;
                    this.transform.eulerAngles = new Vector3(0, v, 0);
                },
                () => this._currentRotation,
                () => this._targetRotation, 7, TargetCurve.Easing);

            // Fade entire canvas in and out.
            this.headerText.color = clearWhite;
            this.resultsText.color = clearWhite;
            this.menuNewSessionText.color = clearWhite;
            this.menuExitGameText.color = clearWhite;
            this.instructionsText.color = clearWhite;
            ColorTarget.AddToTextGUIColor(this.headerText, this.ActiveColor);
            ColorTarget.AddToTextGUIColor(this.resultsText, this.ActiveColor);
            ColorTarget.AddToTextGUIColor(this.menuNewSessionText, this.ActiveColor);
            ColorTarget.AddToTextGUIColor(this.menuExitGameText, this.ActiveColor);
            ColorTarget.AddToTextGUIColor(this.instructionsText, this.ActiveColor);

            this.SetVisible(false);
        }

        private void Update() {
            this.menuNewSessionText.text = Inputs.ReplaceInputNamesInString("<Primary Button>: New session", true);
            this.menuExitGameText.text = Inputs.ReplaceInputNamesInString("<Exit Application>: Exit application", true);
        }

        public void SetVisible(bool value) {
            this._isVisible = value;

            this.menuNewSessionButtonImage.GetComponent<HeldButtonUi>().SetActive(value);
            this.menuExitGameButtonImage.GetComponent<HeldButtonUi>().SetActive(value);

            if (value) {
                var stats = GameData.Instance.CurrentStats;
                var sessionTime = GameData.Instance.TimeInSession;
                var sessionTimeString = sessionTime <= 1 ? "~1" : sessionTime.ToString();

                this.resultsText.text =
                    $"<size={NumberSize}%>{stats.TotalFull10BreathCount}</size>   completed 10 breath cycles\n" +
                    // $"<size={NumberSize}%>{stats.Most10BreathsInASession}</size>   most cycles completed in a session\n" +
                    $"<size={NumberSize}%>{sessionTimeString}</size>   minutes spent in session\n" +
                    $"<size={NumberSize}%>{stats.LatestDailyStreak}</size>   day streak\n";

                this.instructionsText.text = Inputs.ReplaceInputNamesInString(BreathInstructions.Summary, true);

                this.Center(true);
            }
        }

        public void Center(bool instantly) {
            var newCameraRotation = CameraControl.Instance.transform.eulerAngles.y;
            while (newCameraRotation - this._currentRotation > 181) {
                newCameraRotation -= 360;
            }

            while (newCameraRotation - this._currentRotation < -181) {
                newCameraRotation += 360;
            }

            this._targetRotation = newCameraRotation;
            if (instantly) {
                this.transform.eulerAngles = new Vector3(0, newCameraRotation, 0);
            }
        }
        
        public void CompleteNewSession() {
            this.menuNewSessionButtonImage.GetComponent<HeldButtonUi>().Complete(this.menuNewSessionText);
        }
    }
}