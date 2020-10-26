using UnityEngine;

namespace Input {
    public class HeldInput {
        private readonly float _holdSeconds;
        private float? _holdStartTime;

        public bool IsDown => this._holdStartTime.HasValue;
        public bool IsHeldDown => Time.time - this._holdSeconds >= this._holdStartTime;
        public float ProportionDown => this._holdStartTime.HasValue
            ? Mathf.Min(1f, (Time.time - this._holdStartTime.Value) / this._holdSeconds) * .9f + .1f
            : 0f;

        public void SetPressed(bool value) {
            this._holdStartTime = value ? Time.time : (float?) null;
        }
        
        public HeldInput(float holdSeconds) {
            this._holdSeconds = holdSeconds;
            this._holdStartTime = null;
        }
    }
}