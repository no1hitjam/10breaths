using System;
using TMPro;
using UnityEngine;

namespace Ui {
    public class DebugTextUi : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI debugText;

        private float FPS = 0;

        private void Awake() {
            this.gameObject.SetActive(DebugFlags.ShowDebugText);
        }

        private void Update() {
            var deltaTime = Time.deltaTime;
            if (deltaTime > 0) {
                this.FPS = this.FPS * .95f + 1 / deltaTime * .05f;
            }

            this.debugText.text = $"Debug\nFPS: {this.FPS:0}";
        }
    }
}