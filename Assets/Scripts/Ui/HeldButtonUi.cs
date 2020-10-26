using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using Input;
using JetBrains.Annotations;
using TMPro;
using Tween;
using UnityEngine;
using UnityEngine.UI;

namespace Ui {
    [RequireComponent(typeof(Image))]
    public class HeldButtonUi : MonoBehaviour {
        public bool IsActive { get; private set; }
        private HeldInput _heldInput;
        private Coroutine _completionCoroutine;

        public float MinHoldProportion { get; set; }

        public void SetActive(bool value) {
            this.IsActive = value;
            var image = this.GetComponent<Image>();
            image.fillClockwise = !value;
            if (value) {
                image.fillAmount = 0;
            }
        }

        public void Complete([CanBeNull] TextMeshProUGUI associatedText) {
            if (this._heldInput.IsHeldDown) {
                if (this._completionCoroutine != null) {
                    this.StopCoroutine(this._completionCoroutine);
                }

                this._completionCoroutine = this.StartCoroutine(this.CompleteCoroutine(associatedText));
            }

            this.SetActive(false);
        }

        private IEnumerator CompleteCoroutine([CanBeNull] TextMeshProUGUI associatedText) {
            this.GetComponent<FloatTarget>().enabled = false;
            this.GetComponent<Vector3Target>().enabled = false;
            this.GetComponent<Image>().fillAmount = 1;

            // Bounce out.
            var startTime = Time.time;
            var endTime = startTime + .2f;
            if (associatedText != null && associatedText.GetComponent<ColorTarget>() != null) {
                associatedText.GetComponent<ColorTarget>().enabled = false;
            }

            while (Time.time <= endTime) {
                var proportion = (Time.time - startTime) / (endTime - startTime);
                this.transform.localScale = Vector3.one * (1 + .2f * Mathf.Pow(proportion, .5f));
                if (associatedText != null) {
                    associatedText.color = associatedText.color.AsOpaque();
                }

                yield return new WaitForEndOfFrame();
            }

            if (associatedText != null && associatedText.GetComponent<ColorTarget>() != null) {
                associatedText.GetComponent<ColorTarget>().enabled = true;
            }

            // Fall back in.
            var startTime2 = Time.time;
            var endTime2 = startTime2 + .2f;
            while (Time.time <= endTime2) {
                var proportion = (Time.time - startTime2) / (endTime2 - startTime2);
                this.transform.localScale = Vector3.one * (1 + .2f * Mathf.Pow(1 - proportion, .1f));
                yield return new WaitForEndOfFrame();
            }

            this.GetComponent<FloatTarget>().enabled = true;
            this.GetComponent<Vector3Target>().enabled = true;
        }


        public static HeldButtonUi AddTo(Image image, HeldInput heldInput) {
            image.fillAmount = 0;
            image.transform.localScale = Vector3.zero;
            var result = image.gameObject.AddComponent<HeldButtonUi>();
            result._heldInput = heldInput;
            FloatTarget.AddToImageFillAmount(image,
                () => result.IsActive
                    ? Mathf.Max(0, heldInput.ProportionDown * (1 + result.MinHoldProportion) - result.MinHoldProportion)
                    : 0,
                3);
            Vector3Target.AddToLocalScale(image.gameObject,
                () => result.IsActive && heldInput.IsDown ? Vector3.one : Vector3.zero, 5, TargetCurve.Easing);
            return result;
        }
    }
}