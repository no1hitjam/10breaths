using System;
using UnityEngine;
using UnityEngine.UI;

namespace Tween {
    public class FloatTarget : MonoBehaviour {
        private Action<float> _assignValue;
        private Func<float> _getCurrentValue;
        private Func<float> _getNewValue;
        private TargetCurve _curve;
        private float _speed;

        private void Update() {
            var currentValue = this._getCurrentValue();
            var valueDifference = this._getNewValue() - currentValue;

            switch (this._curve) {
                case TargetCurve.Linear:
                    var adjustedMaxSpeed = Time.deltaTime * this._speed;
                    if (Mathf.Abs(valueDifference) > adjustedMaxSpeed) {
                        valueDifference *= adjustedMaxSpeed / Mathf.Abs(valueDifference);
                    }

                    break;
                case TargetCurve.Easing:
                    valueDifference *= Time.deltaTime * this._speed;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            this._assignValue(currentValue + valueDifference);
        }

        public static FloatTarget AddTo(GameObject gameObject, Action<float> assignValue,
            Func<float> getCurrentValue, Func<float> getNewValue, float speed = 1f,
            TargetCurve curve = TargetCurve.Linear) {
            var result = gameObject.AddComponent<FloatTarget>();
            result._assignValue = assignValue;
            result._getCurrentValue = getCurrentValue;
            result._getNewValue = getNewValue;
            result._speed = speed;
            result._curve = curve;
            return result;
        }

        public static FloatTarget AddToImageFillAmount(Image image, Func<float> getNewValue, float speed = 1f,
            TargetCurve curve = TargetCurve.Linear) {
            return AddTo(
                image.gameObject,
                f => image.fillAmount = f,
                () => image.fillAmount,
                getNewValue,
                speed,
                curve
            );
        }
    }
}