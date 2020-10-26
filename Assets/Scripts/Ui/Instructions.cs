using System;
using System.Collections.Generic;
using Helpers;
using Input;
using TMPro;
using Tween;
using UnityEngine;

namespace Ui {
    public class Instructions : MonoBehaviour {
        private const float MaxAngleFromPlayerPov = 40;
        private const float FastSpeed = 2f;
        private const float NormalSpeed = .3f;

        [SerializeField] private TextMeshProUGUI[] textMeshes;

        private bool[] _showMeshArray;

        private float _currentAngleTarget;
        private float _currentAngle;

        public void Clear() {
            foreach (var textMesh in this.textMeshes) {
                textMesh.GetComponent<ColorTarget>().Speed = FastSpeed;
            }

            this._showMeshArray = new bool[] { };
        }

        private void Awake() {
            for (var i = 0; i < this.textMeshes.Length; i++) {
                var textMesh = this.textMeshes[i];
                var index = i;
                ColorTarget.AddToTextGUIColor(textMesh,
                    () => new Color(1, 1, 1,
                        this._showMeshArray != null && this._showMeshArray.Length > index && this._showMeshArray[index]
                            ? 1f
                            : 0f),
                    NormalSpeed);
            }

            FloatTarget.AddTo(this.gameObject, v => {
                    this._currentAngle = v;
                    this.transform.eulerAngles = new Vector3(0, this._currentAngle, 0);
                },
                () => this._currentAngle,
                () => this._currentAngleTarget, 1f, TargetCurve.Easing);
        }

        private void Update() {
            var cameraAngle = CameraControl.Instance.transform.eulerAngles.y;
            var angleFromCamera = AngleHelpers.BetweenPlusMinus180(this._currentAngleTarget - cameraAngle);
            if (Math.Abs(angleFromCamera) > MaxAngleFromPlayerPov) {
                var newAngleOffset = -Mathf.Min(MaxAngleFromPlayerPov / 4, Mathf.Max(-MaxAngleFromPlayerPov / 4, angleFromCamera));
                this._currentAngleTarget += newAngleOffset;
            }
        }

        public void Play(params string[] textStrings) {
            // TODO: The first time, spawn right in front of player.

            foreach (var textMesh in this.textMeshes) {
                textMesh.GetComponent<ColorTarget>().Speed = NormalSpeed;
            }

            this._showMeshArray = new bool[this.textMeshes.Length];
            for (var i = 0; i < textStrings.Length && i < this.textMeshes.Length; i++) {
                if (!string.IsNullOrEmpty(textStrings[i])) {
                    this._showMeshArray[i] = true;
                    this.textMeshes[i].text = Inputs.ReplaceInputNamesInString(textStrings[i], false);
                } else {
                    this._showMeshArray[i] = false;
                }
            }

            for (var i = textStrings.Length; i < this.textMeshes.Length; i++) {
                this._showMeshArray[i] = false;
            }
        }
    }
}