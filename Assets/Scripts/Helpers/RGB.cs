using UnityEngine;

namespace Helpers {
    public readonly struct RGB {
        public readonly float R;
        public readonly float G;
        public readonly float B;

        public RGB(float r, float g, float b) {
            this.R = r;
            this.G = g;
            this.B = b;
        }

        public static RGB One => new RGB(1, 1, 1);
        public static RGB Zero => new RGB(0, 0, 0);

        public static RGB Lerp(RGB a, RGB b, float value) {
            return new RGB(Mathf.Lerp(a.R, b.R, value), Mathf.Lerp(a.G, b.G, value), Mathf.Lerp(a.B, b.B, value));
        }

        public Color ToColor(float alpha = 1.0f) {
            return new Color(this.R, this.G, this.B, alpha);
        }
    }
}