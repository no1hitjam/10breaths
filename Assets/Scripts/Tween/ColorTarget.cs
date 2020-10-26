using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tween {
    public class ColorTarget : MonoBehaviour {
        private Action<Color> _assignValue;
        private Func<Color> _getCurrentValue;
        private Func<Color> _getNewValue;
        private TargetCurve _curve;
        public float Speed { get; set; }

        private void Update() {
            var currentValue = this._getCurrentValue();
            var valueDifference = this._getNewValue() - currentValue;

            switch (this._curve) {
                case TargetCurve.Linear:
                    var adjustedMaxSpeed = Time.deltaTime * this.Speed;
                    if (Mathf.Abs(((Vector4) valueDifference).magnitude) > adjustedMaxSpeed) {
                        valueDifference *= adjustedMaxSpeed / Mathf.Abs(((Vector4) valueDifference).magnitude);
                    }

                    break;
                case TargetCurve.Easing:
                    valueDifference *= Time.deltaTime * this.Speed;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            this._assignValue(currentValue + valueDifference);
        }

        public static ColorTarget AddTo(GameObject gameObject, Action<Color> assignValue,
            Func<Color> getCurrentValue, Func<Color> getNewValue, float speed = 1f,
            TargetCurve curve = TargetCurve.Linear) {
            var result = gameObject.AddComponent<ColorTarget>();
            result._assignValue = assignValue;
            result._getCurrentValue = getCurrentValue;
            result._getNewValue = getNewValue;
            result.Speed = speed;
            result._curve = curve;
            return result;
        }

        public static ColorTarget AddToTextGUIColor(TextMeshProUGUI text, Func<Color> getNewValue, float speed = 1f,
            TargetCurve curve = TargetCurve.Linear) {
            return AddTo(text.gameObject, v => text.color = v, () => text.color, getNewValue, speed, curve);
        }
        
        public static ColorTarget AddToImageColor(Image image, Func<Color> getNewValue, float speed = 1f,
            TargetCurve curve = TargetCurve.Linear) {
            return AddTo(image.gameObject, v => image.color = v, () => image.color, getNewValue, speed, curve);
        }
    }
}