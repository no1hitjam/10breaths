using System.Collections.Generic;
using Audio.WaveFunctions;
using UnityEngine;

namespace Audio {
    public class Instrument {
        public static Instrument Default => new Instrument(new SineWave(), 1, .1f, .3f, .4f, .3f, 2);
        
        private IWaveFunction _waveFunction;
        private float _volume;
        private float _attack;
        private float _decay;
        private float _decayLevel;
        private float _sustain;
        private float _release;

        private readonly List<Note> _currentNotes;

        public Instrument(IWaveFunction waveFunction, float volume, float attack, float decay, float decayLevel,
            float sustain, float release) {
            this._volume = volume;
            this._waveFunction = waveFunction;
            this._attack = attack;
            this._decay = decay;
            this._decayLevel = decayLevel;
            this._sustain = sustain;
            this._release = release;

            this._currentNotes = new List<Note>();
        }

        private float GetNoteVolume(int timeIndex, Note note) {
            return this.GetTimeVolume(timeIndex, note.StartTime) * note.Volume * this._volume * AudioMath.StandardVolume;
        }

        private float GetTimeVolume(int timeIndex, float noteStartTime) {
            var elapsedIndex = (int)(timeIndex - noteStartTime * AudioMath.SampleRate);
            if (elapsedIndex < 0) {
                return 0;
            }

            var attackIndex = this._attack * AudioMath.SampleRate;
            var decayIndex = this._decay * AudioMath.SampleRate;
            var sustainIndex = this._sustain * AudioMath.SampleRate;
            var releaseIndex = this._release * AudioMath.SampleRate;
            
            if (elapsedIndex <= attackIndex) {
                if (attackIndex == 0) {
                    return 1;
                }

                return elapsedIndex / attackIndex;
            }

            if (elapsedIndex <= attackIndex + decayIndex) {
                var decayElapsed = elapsedIndex - attackIndex;
                return (decayIndex - decayElapsed) / decayIndex * (1 - this._decayLevel) + this._decayLevel;
            }

            if (elapsedIndex <= attackIndex + decayIndex + sustainIndex) {
                return this._decayLevel;
            }

            if (elapsedIndex <= attackIndex + decayIndex + sustainIndex + releaseIndex) {
                var releaseElapsed = elapsedIndex - (attackIndex + decayIndex + sustainIndex);
                return (releaseIndex - releaseElapsed) / releaseIndex * this._decayLevel;
            }

            return 0;
        }

        public void PlayNote(int step, float volume = 1f) {
            this._currentNotes.Add(new Note(Time.time + 1, AudioMath.GetFrequency(step), volume));
        }

        public void PlayNote(float frequency, float volume = 1f) {
            this._currentNotes.Add(new Note(Time.time, frequency, volume));
        }

        public float WaveValue(int timeIndex) {
            var value = 0f;
            foreach (var note in this._currentNotes) {
                value += AudioMath.WaveValue(
                    this._waveFunction,
                    null,
                    timeIndex - (int) (note.StartTime * AudioMath.SampleRate),
                    note.Frequency,
                    this.GetNoteVolume(timeIndex, note));
            }

            return value;
        }
    }
}