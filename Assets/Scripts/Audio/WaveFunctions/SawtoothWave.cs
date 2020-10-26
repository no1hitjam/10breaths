using UnityEngine;

namespace Audio.WaveFunctions {
    public class SawtoothWave : IWaveFunction {
        private const int StandardHarmonics = 30;
        private readonly int _harmonics;

        public SawtoothWave(int harmonics = StandardHarmonics) {
            this._harmonics = harmonics;
        }

        public float GetValue(float x) {
            var value = 0f;
            for (var i = 1; i < this._harmonics; i++) {
                value += 1f / i * (i % 2 == 0 ? 1 : -1) * Mathf.Sin(i * x);
            }

            return value;
        }
    }
}