using UnityEngine;

namespace BreathExercise {
    public class BreathToolOrb : MonoBehaviour {
        private const float MaxAlpha = .9f;
        private const float AlphaSpeed = .3f;
        private const float CancelAlphaSpeed = .2f;
        public const float FallAccel = .3f;
        private const float FallToY = .1f;
        private const float AlmostFallToY = .2f;
        private const float ScaleSpeed = .4f;
        private const float ScaleFallSpeed = .14f;
        private const float MeshRendererSize = .25f;
        public static readonly Color OrbColor = new Color(0, .5f, 1f);
        private static readonly Vector3 OrbInitialPlacement = new Vector3(0, 1, 0);
        private static readonly int ColorProperty = Shader.PropertyToID("_Color");

        [SerializeField] private MeshRenderer meshRenderer;
        
        private bool _breathingIn;
        private bool _cancelled;
        private float _alpha;
        private float _fallSpeed;
        private bool _touchedCup;
        private bool _checkedIfTouchedCup;
        private bool _almostTouchedCup;
        private bool _checkedIfAlmostTouchedCup;

        public bool JustTouchedCup {
            get {
                var hasChecked = this._checkedIfTouchedCup;
                if (this._touchedCup) {
                    this._checkedIfTouchedCup = true;
                }

                return this._touchedCup && !hasChecked;
            }
        }
        
        public bool AlmostTouchedCup {
            get {
                var hasChecked = this._checkedIfAlmostTouchedCup;
                if (this._almostTouchedCup) {
                    this._checkedIfAlmostTouchedCup = true;
                }

                return this._almostTouchedCup && !hasChecked;
            }
        }

        private void Awake() {
            this.gameObject.SetActive(false);
        }

        private void Update() {
            if (!this.gameObject.activeInHierarchy) {
                return;
            }

            if (this._cancelled) {
                this._alpha -= CancelAlphaSpeed * Time.deltaTime;
                if (this._alpha <= 0) {
                    Destroy(this.gameObject);
                } else {
                    var color = OrbColor;
                    color.a = this._alpha;
                    this.meshRenderer.material.SetColor(ColorProperty, color);
                }
                return;
            }

            if (this._alpha < MaxAlpha) {
                this._alpha += AlphaSpeed * Time.deltaTime;
                var color = OrbColor;
                color.a = this._alpha;
                this.meshRenderer.material.SetColor(ColorProperty, color);
            }

            if (!this._breathingIn) {
                if (this.transform.localPosition.y > FallToY) {
                    this._fallSpeed += FallAccel * Time.deltaTime;
                    this.transform.localPosition += new Vector3(0, -this._fallSpeed * Time.deltaTime, 0);
                } else {
                    this._touchedCup = true;
                    var depth = ScaleSpeed * Time.deltaTime;
                    var width = Mathf.Pow(depth, 1.1f);
                    this.meshRenderer.transform.localScale -= new Vector3(width, depth, depth);
                    this.transform.localPosition += new Vector3(0, -ScaleFallSpeed * Time.deltaTime, 0);
                    if (this.meshRenderer.transform.localScale.y <= 0) {
                        Destroy(this.gameObject);
                    }
                }

                this._almostTouchedCup = this.transform.localPosition.y < AlmostFallToY;
            }
        }

        public void StartBreatheIn() {
            this._breathingIn = true;
            this._alpha = 0;
            this.transform.localPosition = OrbInitialPlacement;
            var color = OrbColor;
            color.a = this._alpha;
            this.meshRenderer.material.SetColor(ColorProperty, color);
            this.meshRenderer.transform.localScale = Vector3.one * MeshRendererSize;
            this.gameObject.SetActive(true);
        }

        public void StartBreatheOut() {
            this._breathingIn = false;
            this._fallSpeed = 0;
        }

        public void CancelBreatheIn() {
            this._cancelled = true;
        }
    }
}