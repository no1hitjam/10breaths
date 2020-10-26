namespace Helpers {
    public static class AngleHelpers {
        public static float BetweenPlusMinus180(float angle) {
            var a = Between0And360(angle);
            if (a > 180) {
                a -= 360;
            }

            return a;
        }

        public static float Between0And360(float angle) {
            return M.Mod(angle, 360);
        }
    }
}