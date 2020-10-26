using System.Collections;
using Helpers;
using UnityEngine;

namespace Environment {
    public class EnvironmentManager : MonoBehaviour {
        private const float StandardLerpSpeed = .04f * (DebugFlags.FastEnvironmentTransitions ? 5 : 1);
        private const float StandardSunriseTime = 20 * (DebugFlags.FastEnvironmentTransitions ? .4f : 1);

        [SerializeField] private Material skyMaterial;
        [SerializeField] private Material groundMaterial;
        [SerializeField] private FastStarfield starfield;
        [SerializeField] private MountainRange[] mountainRanges;
        [SerializeField] private bool useDebugColor;
        [SerializeField] private SerializableEnvironmentColor debugColor;
        [TextArea] [SerializeField] private string debugColorCodeOutput;

        private EnvironmentColor _previousColor;
        private EnvironmentColor _nextColor;
        private float _currentColorLerp;
        private float _lerpSpeed = StandardLerpSpeed;

        private void Start() {
            this._previousColor = EnvironmentLibrary.BlueDusk;
            this.AssignColor(this._previousColor);
        }

        public void StartDemo() {
            this.StartCoroutine(this.Demo());
        }

        private IEnumerator Demo() {
            while (true) {
                this.skyMaterial.SetVector("_ThroughVector", new Vector4(1, 0, 1, 0));
                yield return new WaitForSeconds(3f);
                this.AssignColor(EnvironmentLibrary.BlueDusk);
                yield return new WaitForSeconds(StandardSunriseTime / 2);
                this.AssignColor(EnvironmentLibrary.FirstPurple);
                yield return new WaitForSeconds(StandardSunriseTime);
                this.AssignColor(EnvironmentLibrary.SecondPurple);
                yield return new WaitForSeconds(StandardSunriseTime);
                this.AssignColor(EnvironmentLibrary.ThirdPurple);
                yield return new WaitForSeconds(StandardSunriseTime);
                this.AssignColor(EnvironmentLibrary.FirstOrange);
                yield return new WaitForSeconds(StandardSunriseTime);
                this.AssignColor(EnvironmentLibrary.Daybreak);
                yield return new WaitForSeconds(StandardSunriseTime);
                this.AssignColor(EnvironmentLibrary.EarlyDay);
                yield return new WaitForSeconds(StandardSunriseTime);
                this.AssignColor(EnvironmentLibrary.Day);
                yield return new WaitForSeconds(StandardSunriseTime * 2);
                this.skyMaterial.SetVector("_ThroughVector", new Vector4(-1, 0, -1, 0));
                yield return new WaitForSeconds(StandardSunriseTime);
                this.AssignColor(EnvironmentLibrary.FirstSunset);
                yield return new WaitForSeconds(StandardSunriseTime);
                this.AssignColor(EnvironmentLibrary.SecondSunset);
                yield return new WaitForSeconds(StandardSunriseTime);
                this.AssignColor(EnvironmentLibrary.ThirdSunset);
                yield return new WaitForSeconds(StandardSunriseTime);
                this.AssignColor(EnvironmentLibrary.LateDusk);
                yield return new WaitForSeconds(StandardSunriseTime);
                this.AssignColor(EnvironmentLibrary.Midnight);
                yield return new WaitForSeconds(StandardSunriseTime);
            }
        }

        private void SetAllThroughVectors(bool sunrise) {
            var throughVector = sunrise ? new Vector4(1, 0, 1, 0) : new Vector4(-1, 0, -1, 0);
            this.skyMaterial.SetVector("_ThroughVector", throughVector);
            foreach (var mountainRange in this.mountainRanges) {
                mountainRange.SetThroughVector(throughVector);
            }
        }

        private void Update() {
            this._currentColorLerp = Mathf.Min(this._currentColorLerp += this._lerpSpeed * Time.deltaTime, 1);
            var currentColor = EnvironmentColor.Lerp(this._previousColor, this._nextColor, this._currentColorLerp);

            if (this.useDebugColor) {
                currentColor = this.debugColor.ToEnvironmentColor();
                this.debugColorCodeOutput = this.debugColor.ToCodeString();
            } else {
                this.debugColor = new SerializableEnvironmentColor(currentColor);
            }

            this.groundMaterial.SetColor("_Color", currentColor.GroundColor);
            this.skyMaterial.SetColor("_Color1", currentColor.skyColor1);
            this.skyMaterial.SetColor("_Color2", currentColor.skyColor2);
            this.skyMaterial.SetColor("_Color3", currentColor.skyColor3);
            this.skyMaterial.SetColor("_Color4", currentColor.skyColor4);
            this.skyMaterial.SetColor("_Color5", currentColor.skyColor5);
            this.skyMaterial.SetFloat("_Proportion", currentColor.proportion);
            this.SetMountainRangeColors(0, currentColor.Mountain1Color1, currentColor.Mountain1Color2);
            this.SetMountainRangeColors(1, currentColor.Mountain2Color1, currentColor.Mountain2Color2);
            this.SetMountainRangeColors(2, currentColor.Mountain3Color1, currentColor.Mountain3Color2);
            if (this.starfield != null) {
                this.starfield.SetColors(currentColor.StarMainColor, currentColor.StarColor1, currentColor.StarColor2,
                    currentColor.skyColor1.Brightness());
            }
        }

        private void SetMountainRangeColors(int index, Color color1, Color color2) {
            if (this.mountainRanges != null && this.mountainRanges.Length > index) {
                this.mountainRanges[index].SetColors(color1, color2);
            }
        }

        public void AssignColor(EnvironmentColor color) {
            this._previousColor = EnvironmentColor.Lerp(this._previousColor, this._nextColor, this._currentColorLerp);
            this._nextColor = color;
            this._currentColorLerp = 0;
        }
    }
}