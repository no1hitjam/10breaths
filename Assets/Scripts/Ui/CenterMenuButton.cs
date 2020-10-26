using System;
using Input;
using UnityEngine;
using UnityEngine.UI;

namespace Ui {
    public class CenterMenuButton : MonoBehaviour {
        [SerializeField] private GameObject centerPressObject;
        [SerializeField] private Image menuFillImage;

        private bool _isCenterActive;
        private float _centerAnimationIndex = 1;
        private bool _centerButtonDown;

        private void Awake() {
            HeldButtonUi.AddTo(this.menuFillImage, Inputs.OptionsButton);
        }

        private void Update() {
            if (this._isCenterActive) {
                if (!this._centerButtonDown && Inputs.OptionsButton.IsDown) {
                    this._centerAnimationIndex = 0;
                    this._centerButtonDown = true;
                } else if (this._centerButtonDown) {
                    if (!Inputs.OptionsButton.IsDown) {
                        this._centerButtonDown = false;
                    }
                }
            }

            // Animate center dot.
            if (this._centerAnimationIndex < 1) {
                this._centerAnimationIndex += 1.5f * Time.deltaTime;
            }

            var value = Mathf.Min(1, Mathf.Max(0, this._centerAnimationIndex));
            this.centerPressObject.transform.localScale = Vector3.one * (1 - Mathf.Pow(2 * value - 1, 2));
        }

        public void SetCenterActive(bool value) {
            this._isCenterActive = value;
            this.menuFillImage.GetComponent<HeldButtonUi>().MinHoldProportion = value ? .25f : 0;
        }
        
        public void SetMenuFillActive(bool value) {
            this.menuFillImage.GetComponent<HeldButtonUi>().SetActive(value);
        }

        public void CompleteFill() {
            this.menuFillImage.GetComponent<HeldButtonUi>().Complete(null);
        }
    }
}