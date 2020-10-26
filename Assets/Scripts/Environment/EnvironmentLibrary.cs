using UnityEngine;

namespace Environment {
    public static class EnvironmentLibrary {
        public static EnvironmentColor Midnight = new EnvironmentColor(new Color(0.00f, 0.02f, 0.11f), 5);

        public static EnvironmentColor BlueDusk = new EnvironmentColor(
            new Color(0.00f, 0.02f, 0.11f),
            new Color(0.05f, 0.03f, 0.23f), 4);

        public static EnvironmentColor FirstPurple = new EnvironmentColor(
            new Color(0.00f, 0.02f, 0.11f),
            new Color(0.02f, 0.05f, 0.17f),
            new Color(0.02f, 0.04f, 0.20f),
            new Color(0.05f, 0.03f, 0.23f),
            new Color(0.13f, 0.00f, 0.34f),
            3.00f);

        public static EnvironmentColor SecondPurple = new EnvironmentColor(
            new Color(0.01f, 0.035f, 0.15f),
            new Color(0.00f, 0.05f, 0.19f), 
            new Color(0.01f, 0.07f, 0.23f), 
            new Color(0.02f, 0.10f, 0.28f),
            new Color(0.07f, 0.14f, 0.32f), 
            2.8f);

        public static EnvironmentColor ThirdPurple = new EnvironmentColor(
            new Color(0.02f, 0.05f, 0.17f),
            new Color(0.0f, 0.06f, 0.23f), 
            new Color(0.01f, 0.1f, 0.37f),
            new Color(0.02f, 0.19f, 0.40f),
            new Color(0.03f, 0.28f, 0.51f),
            2.5f);

        public static EnvironmentColor FirstOrange = new EnvironmentColor(
            new Color(0.05f, 0.1f, 0.23f), 
            new Color(0.12f, 0.18f, 0.43f), 
            new Color(0.25f, 0.35f, 0.62f), 
            new Color(0.91f, 0.81f, 0.68f),
            new Color(0.80f, 0.84f, 0.61f), 
            2f);

        public static EnvironmentColor Daybreak = new EnvironmentColor(
            new Color(0.10f, 0.16f, 0.51f),
            new Color(0.20f, 0.30f, 0.70f),
            new Color(0.26f, 0.51f, 0.76f),
            new Color(0.60f, 0.70f, 0.7f),
            new Color(1.00f, 0.86f, 0.67f), 
            1f);
        
        
        public static EnvironmentColor EarlyDay = new EnvironmentColor(
            new Color(0.15f, 0.2f, 0.61f),
            new Color(0.36f, 0.61f, 0.86f), 
            .8f);
        
        
        public static EnvironmentColor Day = new EnvironmentColor(
            new Color(0.18f, 0.3f, 0.65f),
            0.50f);
        
        
        public static EnvironmentColor FirstSunset = new EnvironmentColor(
            new Color(0.17f, 0.27f, 0.63f),
            new Color(0.56f, 0.41f, 0.56f), 
            0.60f);
        
        
        public static EnvironmentColor SecondSunset = new EnvironmentColor(
            new Color(0.15f, 0.25f, 0.60f),
            new Color(0.96f, 0.41f, 0.56f), 
            0.70f);
        
        
        public static EnvironmentColor ThirdSunset = new EnvironmentColor(
            new Color(0.07f, 0.10f, 0.30f),
            new Color(0.10f, 0.11f, 0.35f),
            new Color(0.27f, 0.12f, 0.40f),
            new Color(0.57f, 0.20f, 0.43f),
            new Color(0.86f, 0.31f, 0.46f), 
            1.5f);

        
        public static EnvironmentColor LateDusk = new EnvironmentColor(
            new Color(0.01f, 0.035f, 0.15f),
            new Color(0.00f, 0.05f, 0.19f), 
            new Color(0.01f, 0.07f, 0.23f), 
            new Color(0.02f, 0.10f, 0.28f),
            new Color(0.07f, 0.14f, 0.32f), 
            2f);
    }
}