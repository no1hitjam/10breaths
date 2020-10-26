using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using Structures;
using UnityEngine;
using Matrix4x4 = UnityEngine.Matrix4x4;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Environment {
    public class FastStarfield : MonoBehaviour {
        private const float AngleRange = 360;
        private const float MinDistance = 4200f;
        private const float MaxDistance = 5000f;
        private const float ScaleMin = 100f;
        private const float ScaleMax = 300f;
        private const float RotationSpeed = .009f;
        private const float JitterAmount = 200;
        private const float IndexBuffer = 1000;

        [SerializeField] private ProportionalMaterial[] starMaterials;
        [SerializeField] private int galaxyStarCount;
        [SerializeField] private int generalStarCount;
        [SerializeField] private Mesh starMesh;

        private float _fullRotation;
        private StarData[][] _starData;
        private List<Matrix4x4[]>[] _bufferedData;

        private void Start() {
            var starData = new List<StarData>[this.starMaterials.Length];
            for (var i = 0; i < starData.Length; i++) {
                starData[i] = new List<StarData>();
            }

            var weightedMaterials =
                new WeightedArray<Material>(F.Map(this.starMaterials, m => (m.material, m.proportion)).ToArray());

            for (var i = 0; i < this.galaxyStarCount; i++) {
                var x = Rand.Value;
                var y = Mathf.Pow(Rand.Value, 2f) * Rand.Sign * 20 + Mathf.Sin(x * Mathf.PI * 4) * 10;
                var z = Rand.Value * 5;
                var newStarData = CreateStarData(new Vector3(x * AngleRange, y, z), weightedMaterials);
                starData[newStarData.MaterialIndex].Add(newStarData);
            }

            for (var i = 0; i < this.generalStarCount; i++) {
                var eulerX = Mathf.Pow(Rand.Value, 4) * 90f * Rand.Sign;
                var eulerY = Rand.Value * 360f;
                var newStarData = CreateStarData(new Vector3(eulerX, eulerY, 0), weightedMaterials);
                starData[newStarData.MaterialIndex].Add(newStarData);
            }

            this._starData = F.Map(starData, l => l.ToArray()).ToArray();
        }

        private void Update() {
            this._fullRotation += RotationSpeed;
            this.transform.eulerAngles = new Vector3(0, 0, this._fullRotation);
            this.BatchAndRender();
        }

        private static StarData CreateStarData(Vector3 eulerAngles, WeightedArray<Material> materials) {
            var materialIndex = Rand.WeightedIndex(materials);
            var distance = Rand.Value > .01f ? Rand.Range(6900, 9000) : Rand.Range(5000, 6000);
            var size = Rand.Range(400, 420 + 200 * 1 - materialIndex / (float) materials.Values.Length);
            if (materialIndex < 2) {
                size += Rand.Range(0, 300);
            }

            return new StarData(eulerAngles, distance, size, materialIndex);
        }

        private void BatchAndRender() {
            if (this._starData == null || this._starData.Length <= 0) {
                return;
            }

            //Clear the batch buffer
            this._bufferedData = new List<Matrix4x4[]>[this._starData.Length];

            for (var materialIndex = 0;
                materialIndex < this._starData.Length &&
                materialIndex < this._bufferedData.Length &&
                materialIndex < this.starMaterials.Length;
                materialIndex++) {
                // Multiple batches.
                var count = this._starData[materialIndex].Length;
                this._bufferedData[materialIndex] = new List<Matrix4x4[]>();
                for (var i = 0; i < count; i += 1023) {
                    if (i + 1023 < count) {
                        var tBuffer = new Matrix4x4[1023];
                        for (var ii = 0; ii < 1023; ii++) {
                            tBuffer[ii] = this._starData[materialIndex][i + ii].RenderData;
                        }

                        this._bufferedData[materialIndex].Add(tBuffer);
                    } else {
                        // Last batch
                        var tBuffer = new Matrix4x4[count - i];
                        for (var ii = 0; ii < count - i; ii++) {
                            tBuffer[ii] = this._starData[materialIndex][i + ii].RenderData;
                        }

                        this._bufferedData[materialIndex].Add(tBuffer);
                    }
                }

                // Draw each batch
                foreach (var batch in this._bufferedData[materialIndex]) {
                    Graphics.DrawMeshInstanced(this.starMesh, 0, this.starMaterials[materialIndex].material, batch,
                        batch.Length);
                }
            }
        }

        [Serializable]
        private class ProportionalMaterial {
            [SerializeField] public Material material;
            [SerializeField] public float proportion;
        }

        private readonly struct StarData {
            private readonly Vector3 _position;
            private readonly Quaternion _rotation;
            private readonly float _size;
            public readonly int MaterialIndex;
            private readonly Matrix4x4 _initialRenderData;

            public StarData(Vector3 eulerAngles, float distance, float size, int materialIndex) {
                this._rotation = Quaternion.Euler(eulerAngles);
                this.MaterialIndex = materialIndex;
                this._size = size;
                this._position = this._rotation * (Vector3.forward * (distance + materialIndex * IndexBuffer)) +
                                 new Vector3(
                                     Rand.Range(-JitterAmount, JitterAmount), Rand.Range(-JitterAmount, JitterAmount),
                                     Rand.Range(-JitterAmount, JitterAmount));
                this._initialRenderData = Matrix4x4.TRS(this._position, this._rotation, Vector3.one * this._size);
            }

            // public Matrix4x4 RenderData => Matrix4x4.TRS(this._position, this._rotation, Vector3.one * this._size);
            public Matrix4x4 RenderData => this._initialRenderData;
        }

        public void SetColors(Color currentColorStarMainColor, Color currentColorStarColor1,
            Color currentColorStarColor2, float brightness) {
            for (var i = 0; i < this.starMaterials.Length; i++) {
                var materialEntry = this.starMaterials[i];
                var mainColor = currentColorStarMainColor;
                var altColor1 = currentColorStarColor1;
                var altColor2 = currentColorStarColor2;

                if (i < 2) {
                    var brightStarColor = Color.Lerp(new Color(.4f, .8f, 1f), Color.white,
                        brightness < .4f ? brightness / .4f : 1);
                    mainColor = Color.Lerp(mainColor, brightStarColor, .8f - i * .4f);
                }

                var brightnessReduction = i / (float) this.starMaterials.Length * .3f;
                brightnessReduction += brightness * 1.2f * (1 + i * .3f);

                materialEntry.material.SetColor("_MainColor", mainColor);
                materialEntry.material.SetColor("_BGColor1", altColor1);
                materialEntry.material.SetColor("_BGColor2", altColor2);
                materialEntry.material.SetFloat("_MaxAlpha", Mathf.Max(0, 1f - brightnessReduction));
            }
        }
    }
}