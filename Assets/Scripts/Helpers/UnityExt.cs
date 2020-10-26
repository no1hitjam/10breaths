using UnityEngine;

namespace Helpers {
    public static class UnityExt {
        public static (Vector2 anchorMin, Vector2 anchorMax) GetAnchorMinAndMax(RectTransform rt) {
            return (rt.anchorMin, rt.anchorMax);
        }
        
        public static void AssignAnchorMinAndMax(RectTransform rt, Vector2 anchorMin, Vector2 anchorMax) {
            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
        }
        
        public static Color AsTransparent(this Color c) {
            return new Color(c.r, c.g, c.b, 0);
        }
        public static Color AsOpaque(this Color c) {
            return new Color(c.r, c.g, c.b, 1);
        }
        
        public static float Brightness(this Color c) {
            return (c.r + c.g + c.b) / 3f;
        }
    }
}