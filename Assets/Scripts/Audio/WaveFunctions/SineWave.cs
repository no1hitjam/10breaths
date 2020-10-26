using UnityEngine;

namespace Audio.WaveFunctions {
    public class SineWave : IWaveFunction {
        public float GetValue(float x) {
            return Mathf.Sin(x);
        }
    }
}