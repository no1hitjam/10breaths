using UnityEngine;

namespace BreathExercise {
    public class BreathToolCup : MonoBehaviour {
        private static readonly Color BaseColor = new Color(0, .01f, .04f);
        private static readonly int ColorProperty = Shader.PropertyToID("_Color");
        
        private MeshRenderer _meshRenderer;
        private float _alpha;
        public bool IsVisible { get; set; }
        public bool IsOpaque { get; set; }

        private float TargetAlpha => this.IsVisible ? this.IsOpaque ? 1 : .75f : 0;
        
        private void Awake() {
            this._meshRenderer = this.GetComponentInChildren<MeshRenderer>();
        }

        private void Update() {
            this._alpha += (this.TargetAlpha - this._alpha) * 2f * Time.deltaTime;
            var color = BaseColor;
            color.a = this._alpha;
            this._meshRenderer.material.SetColor(ColorProperty, color);
        }
    }
}