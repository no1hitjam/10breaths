using System;
using Audio.WaveFunctions;
using JetBrains.Annotations;
using UnityEngine;

namespace Audio {
    [RequireComponent(typeof(AudioSource))]
    public class DroneSource : MonoBehaviour {
        public readonly struct Instrument {
            public readonly IWaveFunction WaveFunction;
            public readonly float Volume;
            public readonly int StepOffset;

            public Instrument(IWaveFunction waveFunction, float volume = 1, int stepOffset = 0) {
                this.WaveFunction = waveFunction;
                this.Volume = volume;
                this.StepOffset = stepOffset;
            }
        }

        private AudioSource _audioSource;
        private int _timeIndex = 0;
        private Instrument[] _instruments;
        private float _frequency;
        
        public int TargetStep { get; set; }
        public float Bend { get; set; }
        public float Volume { get; set; } = 1;
        
        private void Awake() {
            this._audioSource = this.GetComponent<AudioSource>();
            this._audioSource.playOnAwake = false;
            this._audioSource.Stop();
        }

        private void Update() {
            // TODO: Bend;
            this._frequency = AudioMath.GetFrequency(this.TargetStep);
        }
        

        private void OnAudioFilterRead(float[] data, int channels) {
            for (var i = 0; i < data.Length; i += channels) {
                for (var channelIndex = 0; channelIndex < channels; channelIndex++) {
                    data[i + channelIndex] = this.WaveValue(this._timeIndex);
                }

                this._timeIndex++;
            }
        }

        private float WaveValue(int timeIndex) {
            var value = 0f;
            for (var i = 0; i < this._instruments.Length; i++) {
                var instrument = this._instruments[i];
                value += AudioMath.WaveValue(
                    instrument.WaveFunction,
                    null,
                    timeIndex,
                    AudioMath.GetFrequency(this.TargetStep + instrument.StepOffset),
                    this.Volume * instrument.Volume * AudioMath.StandardVolume * (1 + .5f * Mathf.Sin(timeIndex * .00002f / (i + 1))));
            }

            return value;
        }

        public static DroneSource Create(Transform parent, int startingStep, [CanBeNull] Instrument[] instruments = null) {
            var gameObject = new GameObject();
            gameObject.transform.SetParent(parent);
            gameObject.AddComponent<AudioSource>();
            var proceduralSource = gameObject.AddComponent<DroneSource>();

            proceduralSource._instruments = instruments ?? new[] {new Instrument(new SineWave())};
            proceduralSource.TargetStep = startingStep;

            return proceduralSource;
        }
    }
}