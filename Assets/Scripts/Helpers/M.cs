namespace Helpers {
    public class M {
        public static int Mod(int x, int m) {
            var r = x%m;
            return r<0 ? r+m : r;
        }
        
        public static float Mod(float x, float m) {
            var r = x%m;
            return r<0 ? r+m : r;
        }
    }
}