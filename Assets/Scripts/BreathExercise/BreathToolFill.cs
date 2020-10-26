using UnityEngine;

namespace BreathExercise {
    public class BreathToolFill : MonoBehaviour {
        public const int TotalFills = 10;
        
        public static readonly Color PerfectColor = new Color(1, 1, .4f);
        
        private float _fallSpeed;
        public int FillAmount { get; private set; }
        public bool IsContained { get; set; } = true;

        public bool IsOverflowing => this.FillAmount > TotalFills;
        public bool IsPerfectlyFilled => this.FillAmount == TotalFills;

        private Color _color;
        private static readonly int ColorProperty = Shader.PropertyToID("_Color");
        public Color TargetColor => this.FillAmount == TotalFills ? PerfectColor : BreathToolOrb.OrbColor; 

        private void Awake() {
            this.gameObject.SetActive(false);
        }

        private void Update() {
            var thisTransform = this.transform;
            var targetFillScale = Mathf.Pow((float) this.FillAmount / TotalFills, .75f);
            var targetFillSideScale = Mathf.Pow(targetFillScale, .35f);
            var targetFillVector = new Vector3(targetFillSideScale, targetFillScale, targetFillSideScale);
            var fillLocalScaleVector = thisTransform.localScale;
            fillLocalScaleVector += (targetFillVector - fillLocalScaleVector) * 1f * Time.deltaTime;
            thisTransform.localScale = fillLocalScaleVector;

            if (!this.IsContained) {
                this._fallSpeed += BreathToolOrb.FallAccel * Time.deltaTime;
                thisTransform.localPosition += new Vector3(0, -this._fallSpeed * Time.deltaTime, 0);

                if (thisTransform.localPosition.y < -100) {
                    Destroy(this.gameObject);
                }
            }

            this._color = Color.Lerp(this._color, this.TargetColor, .1f);
            this.GetComponentInChildren<MeshRenderer>().material.SetColor(ColorProperty, this._color);
        }

        public void IncrementFillAmount() {
            this.FillAmount++;
            if (!this.gameObject.activeInHierarchy) {
                this.gameObject.SetActive(true);
                this.transform.localScale = new Vector3(0, 0, 0);
            }
        }

        public void Clear() {
            this.FillAmount = 0;
        }
    }
}