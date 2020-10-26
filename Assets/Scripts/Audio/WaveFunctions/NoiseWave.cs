using Helpers;
using UnityEngine;
using UnityEngine.Assertions;

namespace Audio.WaveFunctions {
    public class NoiseWave : IWaveFunction {
        private const float StandardNoiseIntensity = 1f;
        
        private readonly float _noiseIntensity;

        public NoiseWave(float noiseIntensity = StandardNoiseIntensity) {
            Assert.IsTrue(Mathf.Abs(noiseIntensity) <= 1);
            this._noiseIntensity = noiseIntensity;
        }

        public float GetValue(float x) {
            var value = Mathf.Sin(x);
            var offset = (Rand.Value * 4 - 2) * this._noiseIntensity;
            if (value + offset > 1) {
                offset = 1 - (value + offset);
                value = 1;
            }

            if (value + offset < -1) {
                offset = -1 - (value + offset);
                value = -1;
            }

            return value + offset;
        }
    }
}