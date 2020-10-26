using System;
using System.Collections.Generic;
using Audio.WaveFunctions;
using Structures;
using UnityEngine;

namespace Audio {
    public static class AudioMath {
        public const float StandardVolume = .2f;
        public const float SampleRate = 44100;

        // From A4.
        public static float GetFrequency(int step) {
            return 440 * Mathf.Pow(Mathf.Pow(2, 1 / 12f), step);
        }

        public static float WaveValue(IWaveFunction baseWave, IEnumerable<(IWaveFunction, float)> waveAdditions,
            int timeIndex, float frequency, float volume) {
            var baseWaveValue = WaveValue(baseWave, timeIndex, frequency, volume);

            waveAdditions = waveAdditions ?? new (IWaveFunction, float)[] { };
            foreach (var (waveType, amount) in waveAdditions) {
                baseWaveValue = Mathf.Lerp(baseWaveValue, WaveValue(waveType, timeIndex, frequency, volume), amount);
            }

            return baseWaveValue;
        }

        private static float WaveValue(IWaveFunction wave, int timeIndex, float frequency, float volume) {
            return WaveValue(wave, timeIndex, frequency) * volume;
        }

        private static float WaveValue(IWaveFunction wave, int timeIndex, float frequency) {
            var x = 2 * Mathf.PI * timeIndex * frequency / SampleRate;
            return wave.GetValue(x);
        }
    }
}