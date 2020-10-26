using System;
using UnityEngine;

namespace Tween {
    public class Vector3Target : MonoBehaviour {
        private Action<Vector3> _assignValue;
        private Func<Vector3> _getCurrentValue;
        private Func<Vector3> _getNewValue;
        private TargetCurve _curve;
        private float _speed;

        private void Update() {
            var currentValue = this._getCurrentValue();
            var valueDifference = this._getNewValue() - currentValue;
            
            switch (this._curve) {
                case TargetCurve.Linear:
                    var adjustedMaxSpeed = Time.deltaTime * this._speed;
                    if (Mathf.Abs(valueDifference.magnitude) > adjustedMaxSpeed) {
                        valueDifference *= adjustedMaxSpeed / Mathf.Abs(valueDifference.magnitude);
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

        public static Vector3Target AddTo(GameObject gameObject, Action<Vector3> assignValue,
            Func<Vector3> getCurrentValue, Func<Vector3> getNewValue, float speed = 1f,
            TargetCurve curve = TargetCurve.Linear) {
            var result = gameObject.AddComponent<Vector3Target>();
            result._assignValue = assignValue;
            result._getCurrentValue = getCurrentValue;
            result._getNewValue = getNewValue;
            result._speed = speed;
            result._curve = curve;
            return result;
        }

        public static Vector3Target AddToLocalScale(GameObject gameObject, Func<Vector3> getNewValue, float speed = 1f,
            TargetCurve curve = TargetCurve.Linear) {
            return AddTo(
                gameObject,
                v => gameObject.transform.localScale = v,
                () => gameObject.transform.localScale,
                getNewValue,
                speed,
                curve
            );
        }
    }
}