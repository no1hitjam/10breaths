using UnityEngine;

namespace Environment {
    [System.Serializable]
    public class SerializableEnvironmentColor {
        [SerializeField] public Color skyColor1;
        [SerializeField] public Color skyColor2;
        [SerializeField] public Color skyColor3;
        [SerializeField] public Color skyColor4;
        [SerializeField] public Color skyColor5;
        [SerializeField] public float proportion;

        public SerializableEnvironmentColor(EnvironmentColor currentColor) {
            this.skyColor1 = currentColor.skyColor1;
            this.skyColor2 = currentColor.skyColor2;
            this.skyColor3 = currentColor.skyColor3;
            this.skyColor4 = currentColor.skyColor4;
            this.skyColor5 = currentColor.skyColor5;
            this.proportion = currentColor.proportion;
        }

        public EnvironmentColor ToEnvironmentColor() {
            return new EnvironmentColor(this.skyColor1, this.skyColor2, this.skyColor3, this.skyColor4, this.skyColor5,
                this.proportion);
        }

        private string ColorToCodeString(Color c) {
            return $"new Color({c.r:0.00}f, {c.g:0.00}f, {c.b:0.00}f)";
        }

        public string ToCodeString() {
            return
                $"new EnvironmentColor({this.ColorToCodeString(this.skyColor1)}, {this.ColorToCodeString(this.skyColor2)}, {this.ColorToCodeString(this.skyColor3)}, {this.ColorToCodeString(this.skyColor4)}, {this.ColorToCodeString(this.skyColor5)}, {this.proportion:0.00}f);";
        }
    }
}