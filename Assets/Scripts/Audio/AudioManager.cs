using System;
using System.Collections;
using Audio.WaveFunctions;
using Structures;
using UnityEngine;

namespace Audio {
    public class AudioManager : MonoBehaviour {
        private ProceduralSource[] _sources;

        private DroneSource[] _droneSources;

        private void Start() {
            // this._sources = new[] {
            //     ProceduralSource.Create(this.transform, -24, new SawtoothWave()),
            //     ProceduralSource.Create(this.transform, -20, new SawtoothWave()),
            //     ProceduralSource.Create(this.transform, -36, new SawtoothWave()),
            //     ProceduralSource.Create(this.transform, -32, new SineWave()),
            // };
            this._sources = new[] {
                ProceduralSource.Create(this.transform),
            };

            this._droneSources = new[] {
                DroneSource.Create(this.transform, -39, new []{
                    new DroneSource.Instrument(new NoiseWave(), .3f, 24),
                    new DroneSource.Instrument(new SawtoothWave(4), .4f),
                    new DroneSource.Instrument(new SawtoothWave(2), .2f, 28),
                    new DroneSource.Instrument(new SawtoothWave(1), .4f, 7),
                    new DroneSource.Instrument(new SawtoothWave(1), 1.5f, -12),
                    new DroneSource.Instrument(new SawtoothWave(1), .4f, 12),
                    new DroneSource.Instrument(new SawtoothWave(1), .5f, -5),
                    new DroneSource.Instrument(new SawtoothWave(1), .7f, 31),
                    new DroneSource.Instrument(new SawtoothWave(1), 2f, -12),
                    new DroneSource.Instrument(new SawtoothWave(1), .8f, 24),
                }),
            };

            this.StartCoroutine(this.PlayMusic());
        }

        private void Update() {
            
        }

        private IEnumerator PlayMusic() {
            // while (true) {
            //     var notes = new NonEmptyArray<int>( new[] {-24, -20, -18, -16, -13});
            //     foreach (var source in this._sources) {
            //         source.Instrument.PlayNote(Rand.Element(notes));
            //     }
            //
            //     yield return new WaitForSeconds(Rand.Range(.5f, 3f));
            // }
            yield break;
        }
    }
}