using Helpers;
using UnityEngine;

namespace Environment {
    public class Starfield : MonoBehaviour {
        private const float AngleRange = 360;
        private const float ScaleMin = 200f;
        private const float ScaleMax = 300f;
        private const float RotationSpeed = .009f;

        [SerializeField] private GameObject starPrefab;
        private static readonly int MainColor = Shader.PropertyToID("_MainColor");

        private float _fullRotation;
    
        private void Start() {
            for (var i = 0; i < 4000; i++) {
                var x = Rand.Value;
                var y = Mathf.Pow(Rand.Value, 2f) * Rand.Sign * 20 + Mathf.Sin(x * Mathf.PI * 4) * 10;
                var z = Rand.Value * 5;
                this.CreateChildStar(new Vector3(x * AngleRange, y, z));
            }
        
            for (var i = 0; i < 8000; i++) {
                var x = Rand.Value;
                var y = Rand.Value * .1f;
                var z = Mathf.Pow(Rand.Value, .3f) * Rand.Sign;
                this.CreateChildStar(new Vector3(x * Rand.Sign * AngleRange + 29, y * Rand.Sign * AngleRange, z * AngleRange));
            }
        }

        private void Update() {
            this._fullRotation += RotationSpeed;
            this.transform.eulerAngles = new Vector3(0, 0, this._fullRotation);
        }

        private void CreateChildStar(Vector3 eulerAngles) {
            var newStar = Instantiate(this.starPrefab, this.transform);
            newStar.transform.eulerAngles = eulerAngles;
            var childRenderer = newStar.GetComponentInChildren<MeshRenderer>();
            var sizeProportion = Mathf.Pow(Rand.Value, 40);
            if (Rand.Value < .001f) {
                sizeProportion = 2;
            }

            childRenderer.transform.localScale = Vector3.one * (sizeProportion * (ScaleMax - ScaleMin) + ScaleMin);
            var intensity = (1 -  Mathf.Pow(Rand.Value, .1f)) * .6f + sizeProportion * .6f;
            childRenderer.material.SetColor(MainColor, new Color(1f, 1f, 1f, intensity));
        }
    }
}