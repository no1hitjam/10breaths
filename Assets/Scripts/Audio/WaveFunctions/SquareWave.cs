using UnityEngine;

namespace Audio.WaveFunctions {
    public class SquareWave : IWaveFunction {
        public float GetValue(float x) {
            return Mathf.Sin(x) > 0 ? 1 : -1;
        }
    }
}