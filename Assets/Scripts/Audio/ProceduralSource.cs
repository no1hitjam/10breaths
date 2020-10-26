using System;
using System.Collections.Generic;
using Audio.WaveFunctions;
using JetBrains.Annotations;
using UnityEngine;

namespace Audio {
    [RequireComponent(typeof(AudioSource))]
    public class ProceduralSource : MonoBehaviour {
        private AudioSource _audioSource;
        private int _timeIndex = 0;

        public Instrument Instrument;

        private void Awake() {
            this._audioSource = this.GetComponent<AudioSource>();
            this._audioSource.playOnAwake = false;
            this._audioSource.Stop();
        }

        private void OnAudioFilterRead(float[] data, int channels) {
            for (var i = 0; i < data.Length; i += channels) {
                for (var channelIndex = 0; channelIndex < channels; channelIndex++) {
                    data[i + channelIndex] = this.Instrument.WaveValue(this._timeIndex);
                }

                this._timeIndex++;
            }
        }

        public static ProceduralSource Create(Transform parent, [CanBeNull] Instrument instrument = null) {
            var gameObject = new GameObject();
            gameObject.transform.SetParent(parent);
            gameObject.AddComponent<AudioSource>();
            var proceduralSource = gameObject.AddComponent<ProceduralSource>();

            proceduralSource.Instrument = instrument ?? Instrument.Default;

            return proceduralSource;
        }
    }
}