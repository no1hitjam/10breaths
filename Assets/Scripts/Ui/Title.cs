using Helpers;
using Input;
using TMPro;
using Tween;
using UnityEngine;
using UnityEngine.UI;

namespace Ui {
    public class Title : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI startButtonText;
        [SerializeField] private Image startButtonImage;
        [SerializeField] private TextMeshProUGUI exitButtonText;
        [SerializeField] private Image exitButtonImage;

        private bool _isActive;

        private void Awake() {
            this.titleText.color = this.titleText.color.AsTransparent();
            this.startButtonText.color = this.startButtonText.color.AsTransparent();
            this.exitButtonText.color = this.exitButtonText.color.AsTransparent();

            ColorTarget.AddToTextGUIColor(this.titleText,
                () => this._isActive ? this.titleText.color.AsOpaque() : this.titleText.color.AsTransparent());
            ColorTarget.AddToTextGUIColor(this.startButtonText,
                () => this._isActive ? this.startButtonText.color.AsOpaque() : this.startButtonText.color.AsTransparent());
            ColorTarget.AddToTextGUIColor(this.exitButtonText,
                () => this._isActive ? this.exitButtonText.color.AsOpaque() : this.exitButtonText.color.AsTransparent());
        
        
            HeldButtonUi.AddTo(this.startButtonImage, Inputs.PrimaryButton);
            HeldButtonUi.AddTo(this.exitButtonImage, Inputs.ExitButton);
        }

        private void Update() {
            if (!this._isActive) {
                return;
            }

            this.startButtonText.text =
                Inputs.ReplaceInputNamesInString("Hold <Primary Button> to start", false);
            this.exitButtonText.text =
                Inputs.ReplaceInputNamesInString("Hold <Exit Application> to exit", false);
        }

        public void Show() {
            this._isActive = true;
            this.SetTransitionSpeed(.5f);
            this.startButtonImage.GetComponent<HeldButtonUi>().SetActive(true);
            this.exitButtonImage.GetComponent<HeldButtonUi>().SetActive(true);
        }

        public void TransitionIntoSession() {
            this.SetTransitionSpeed(1f);
            this.startButtonImage.GetComponent<HeldButtonUi>().Complete(this.startButtonText);
            this.exitButtonImage.GetComponent<HeldButtonUi>().SetActive(false);
            this._isActive = false;
        }

        private void SetTransitionSpeed(float speed) {
            this.titleText.GetComponent<ColorTarget>().Speed = speed;
            this.startButtonText.GetComponent<ColorTarget>().Speed = speed;
            this.exitButtonText.GetComponent<ColorTarget>().Speed = speed;
        }
    }
}