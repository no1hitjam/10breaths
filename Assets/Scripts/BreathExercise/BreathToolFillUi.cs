using System;
using TMPro;
using Tween;
using UnityEngine;
using UnityEngine.UI;

namespace BreathExercise {
    public class BreathToolFillUi : MonoBehaviour {
        private const float FadeSpeed = .2f;
        
        private static readonly Color VisibleColor = new Color(.9f, .98f, 1, .4f);
        private static readonly Color InvisibleColor = new Color(VisibleColor.r, VisibleColor.g, VisibleColor.b, 0);
        
        [SerializeField] private TextMeshProUGUI numerator;
        [SerializeField] private TextMeshProUGUI denominator;
        [SerializeField] private Image dividingLine;

        public bool IsActive { get; set; } = true;
        private bool _visible; 

        private void Start() {
            this.numerator.color = InvisibleColor;
            this.denominator.color = InvisibleColor;
            this.dividingLine.color = InvisibleColor;
            ColorTarget.AddToTextGUIColor(this.numerator, () => this.IsActive && this._visible ? VisibleColor : InvisibleColor, FadeSpeed);
            ColorTarget.AddToTextGUIColor(this.denominator, () => this.IsActive && this._visible ? VisibleColor : InvisibleColor, FadeSpeed);
            ColorTarget.AddToImageColor(this.dividingLine, () => this.IsActive && this._visible ? VisibleColor : InvisibleColor, FadeSpeed);
        }

        public void SetValues(int numeratorValue, int denominatorValue) {
            this.numerator.text = numeratorValue.ToString();
            this.denominator.text = denominatorValue.ToString();
            this._visible = numeratorValue > 0;
        }

        public void Hide() {
            this._visible = false;
        }
    }
}