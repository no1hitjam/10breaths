using System.Collections;
using Data;
using Helpers;
using Input;
using Tween;
using Ui;
using UnityEngine;
using UnityEngine.UI;

namespace BreathExercise {
    public class BreathTool : MonoBehaviour {
        private const int TutorialCount = 1;
        private const float MinBreathSeconds = 1f;

        [SerializeField] private BreathToolCup cup;
        [SerializeField] private GameObject orbPrefab;
        [SerializeField] private GameObject cupFillPrefab;
        [SerializeField] private Instructions instructions;
        [SerializeField] private Image releaseButtonImage;
        [SerializeField] private BreathToolFillUi breathToolFillUi;
        [SerializeField] private CenterMenuButton centerMenuButton;

        private bool _isActive = false;
        private float? _breathingInTime;
        private bool _isContainingBreaths;
        private bool _breathingOut;
        private BreathToolOrb _currentOrb;
        private BreathToolFill _currentCupFill;
        private float? _introductionStartTime;
        private float _currentAngle;
        private float _currentAngleTarget;

        private int CurrentFillAmount => this._currentCupFill == null ? 0 : this._currentCupFill.FillAmount;
        private bool IsPerfectlyFilled => this._currentCupFill != null && this._currentCupFill.IsPerfectlyFilled;

        private bool TakenMinBreathIn => this._breathingInTime.HasValue &&
                                         Time.time - this._breathingInTime.Value >= MinBreathSeconds;

        private bool IsReleasingAllowed =>
            GameData.Instance.TutorialIndex > 0 || this.CurrentFillAmount == BreathToolFill.TotalFills;

        private void Awake() {
            HeldButtonUi.AddTo(this.releaseButtonImage, Inputs.SecondaryButton);

            FloatTarget.AddTo(this.gameObject, v => {
                    this._currentAngle = v;
                    this.transform.eulerAngles = new Vector3(0, v, 0);
                },
                () => this._currentAngle,
                () => this._currentAngleTarget, 5f, TargetCurve.Easing);
        }

        private void Update() {
            if (!this._isActive) {
                return;
            }

            if (!DebugFlags.DebugDontShowTutorial) {
                this.instructions.Play(BreathInstructions.Get(GameData.Instance.TutorialIndex, this.CurrentFillAmount,
                    !this.TakenMinBreathIn && !this._breathingOut));
            }

            if (this._breathingInTime.HasValue) {
                if (!Inputs.PrimaryButton.IsDown) {
                    if (this.TakenMinBreathIn) {
                        this.StartBreatheOut();
                    } else {
                        this.CancelBreatheIn();
                    }
                }
            } else {
                if (this._breathingOut) {
                    if (this._currentOrb != null &&
                        ((this.CurrentFillAmount > 0 && this._currentOrb.JustTouchedCup) ||
                         (this.CurrentFillAmount <= 0 && this._currentOrb.AlmostTouchedCup))) {
                        this.FinishBreatheOut();
                    }
                } else {
                    if (Inputs.PrimaryButton.IsDown) {
                        if (GameData.Instance.TutorialIndex != 0 || !this.IsPerfectlyFilled) {
                            this.StartBreatheIn();
                            if (!this._isContainingBreaths) {
                                this.StartContainingBreaths();
                            }
                        }
                    }
                }
            }

            if (this._isContainingBreaths) {
                if ((Inputs.SecondaryButton.IsHeldDown && this.IsReleasingAllowed && !this._breathingOut &&
                     !this._breathingInTime.HasValue) || this._currentCupFill.IsOverflowing) {
                    this.StopContainingBreaths();
                }
            }
        }

        private void StartContainingBreaths() {
            this._isContainingBreaths = true;
            this._currentCupFill =
                Instantiate(this.cupFillPrefab, this.cup.transform).GetComponent<BreathToolFill>();
            this.cup.IsVisible = true;
            var earlyGameEasyMode = GameData.Instance.IntroductionIndex < 2;
            this.cup.IsOpaque = GameData.Instance.Current10BreathCount > (earlyGameEasyMode ? 1 : 0);
            this.breathToolFillUi.IsActive = earlyGameEasyMode && !this.cup.IsOpaque;
            this.releaseButtonImage.GetComponent<HeldButtonUi>().SetActive(this.IsReleasingAllowed);
            this.Center(true);
            this.centerMenuButton.SetCenterActive(true);
        }

        private void StopContainingBreaths() {
            this._isContainingBreaths = false;
            if (this._currentCupFill != null) {
                if (this._currentCupFill.FillAmount == BreathToolFill.TotalFills) {
                    GameData.Instance.Increment10BreathCount();

                    if (GameData.Instance.TutorialIndex == 0) {
                        GameData.Instance.TutorialIndex++;
                    }
                }

                this._currentCupFill.IsContained = false;
                this._currentCupFill.transform.SetParent(null, true);
                this._currentCupFill = null;
                this.breathToolFillUi.Hide();
            }

            this.cup.IsVisible = false;
            this.releaseButtonImage.GetComponent<HeldButtonUi>().Complete(null);
            this.centerMenuButton.SetCenterActive(false);
        }

        private void StartBreatheIn() {
            this._breathingInTime = Time.time;
            this._currentOrb = Instantiate(this.orbPrefab, this.cup.transform).GetComponent<BreathToolOrb>();
            this._currentOrb.StartBreatheIn();
        }

        private void StartBreatheOut() {
            this._breathingInTime = null;
            this._breathingOut = true;
            this._currentOrb.StartBreatheOut();
        }

        private void CancelBreatheIn() {
            this._breathingInTime = null;
            this._currentOrb.CancelBreatheIn();
            this._currentOrb = null;
        }

        private void FinishBreatheOut() {
            if (this._isContainingBreaths) {
                this._currentCupFill.IncrementFillAmount();
                this.breathToolFillUi.SetValues(this._currentCupFill.FillAmount, BreathToolFill.TotalFills);
                this._breathingOut = false;
                this.releaseButtonImage.GetComponent<HeldButtonUi>().SetActive(this.IsReleasingAllowed);
            }
        }

        public void SetActive(bool value) {
            this.instructions.Clear();

            this.StopContainingBreaths();
            if (this._currentCupFill != null) {
                this._currentCupFill.Clear();
            }

            if (this._currentOrb != null) {
                Destroy(this._currentOrb);
                this._currentOrb = null;
            }

            if (value) {
                this.StartCoroutine(this.StartIntroduction());
            } else {
                this._isActive = false;
            }
        }

        private IEnumerator StartIntroduction() {
            if (!GameData.Instance.IsFirstSessionAfterOpening ||
                GameData.Instance.IntroductionIndex >= BreathToolIntroductions.Scripts.Length ||
                DebugFlags.DebugDontShowIntroductions) {
                this._isActive = true;
                yield break;
            }

            var introductionScript = BreathToolIntroductions.Scripts[GameData.Instance.IntroductionIndex];
            switch (GameData.Instance.IntroductionIndex) {
                case 0:
                    this.instructions.Play(introductionScript[0]);
                    yield return new WaitForSeconds(4f);
                    this.instructions.Play(introductionScript[0], introductionScript[1]);
                    yield return new WaitForSeconds(8f);
                    this.instructions.Clear();
                    yield return new WaitForSeconds(3f);
                    this.instructions.Play(introductionScript[2]);
                    yield return new WaitForSeconds(8f);
                    this.instructions.Clear();
                    yield return new WaitForSeconds(3f);
                    this.instructions.Play(introductionScript[3]);
                    yield return new WaitForSeconds(3f);
                    this.instructions.Play(introductionScript[3], introductionScript[4]);
                    yield return new WaitForSeconds(10f);
                    break;
                case 1:
                    this.instructions.Play(introductionScript[0]);
                    yield return new WaitForSeconds(8f);
                    this.instructions.Clear();
                    yield return new WaitForSeconds(3f);
                    this.instructions.Play(introductionScript[1]);
                    yield return new WaitForSeconds(3f);
                    this.instructions.Play(introductionScript[1], introductionScript[2]);
                    yield return new WaitForSeconds(8f);
                    this.instructions.Clear();
                    yield return new WaitForSeconds(3f);
                    this.instructions.Play(introductionScript[3]);
                    yield return new WaitForSeconds(2.5f);
                    this.instructions.Play(introductionScript[3], introductionScript[4]);
                    yield return new WaitForSeconds(8f);
                    this.instructions.Clear();
                    yield return new WaitForSeconds(3f);
                    this.instructions.Play(introductionScript[5]);
                    yield return new WaitForSeconds(2.5f);
                    this.instructions.Play(introductionScript[5], introductionScript[6]);
                    yield return new WaitForSeconds(8f);
                    break;
                case 2:
                    this.instructions.Play(introductionScript[0]);
                    yield return new WaitForSeconds(8f);
                    this.instructions.Clear();
                    yield return new WaitForSeconds(3f);
                    this.instructions.Play(introductionScript[1]);
                    yield return new WaitForSeconds(2.5f);
                    this.instructions.Play(introductionScript[1], introductionScript[2]);
                    yield return new WaitForSeconds(8f);
                    break;
            }

            GameData.Instance.IntroductionIndex++;
            this.instructions.Clear();
            yield return new WaitForSeconds(3f);
            this._isActive = true;
        }


        public void Center(bool instantly) {
            var angleDiff =
                AngleHelpers.BetweenPlusMinus180(CameraControl.Instance.transform.eulerAngles.y -
                                                 this._currentAngleTarget);
            this._currentAngleTarget += angleDiff;
            if (instantly) {
                this._currentAngle = this._currentAngleTarget;
                this.transform.eulerAngles = new Vector3(0, this._currentAngleTarget, 0);
            }
        }
    }
}