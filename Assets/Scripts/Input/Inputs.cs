using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input {
    [RequireComponent(typeof(PlayerInput))]
    public class Inputs : MonoBehaviour {
        private const float MenuButtonHoldSeconds = 2;

        [SerializeField] private InputActionAsset inputActionAsset;

        private string[] _inputActionNameOrder;

        private static Inputs _instance;

        public static readonly HeldInput PrimaryButton = new HeldInput(1.5f);
        public static readonly HeldInput SecondaryButton = new HeldInput(.8f);
        public static readonly HeldInput OptionsButton = new HeldInput(2.5f);
        public static readonly HeldInput ExitButton = new HeldInput(2.5f);

        public static Vector2 LookDelta { get; private set; }

        [NotNull] private static string[] _allInputNames = {};

        private void Awake() {
            _instance = this;
        }

        private void Start() {
            var actions = this.GetComponent<PlayerInput>().currentActionMap.actions;
            this._inputActionNameOrder = new string[actions.Count];
            for (var i = 0; i < actions.Count; i++) {
                this._inputActionNameOrder[i] = actions[i].name;
            }

            _allInputNames = actions.Select(action => action.name).ToArray();
        }

        public static string ReplaceInputNamesInString(string s, bool shortened) {
            foreach (var inputName in _allInputNames) {
                var buttonNameString = GetButtonName(inputName, shortened);
                if (buttonNameString.StartsWith("the ")) {
                    buttonNameString = buttonNameString.Insert(4, "<b>");
                } else {
                    buttonNameString = "<b>" + buttonNameString;
                }

                buttonNameString += "</b>";

                s = s.Replace($"<{inputName}>", buttonNameString);
            }

            return s;
        }

        private InputBinding? GetActionBinding(int index) {
            var currentControlScheme = this.GetComponent<PlayerInput>().currentControlScheme;
            var action = this.GetComponent<PlayerInput>().currentActionMap.actions[index];
            foreach (var binding in action.bindings) {
                if (binding.groups.Contains(currentControlScheme)) {
                    return binding;
                }
            }

            return null;
        }

        private int GetActionIndex(string actionName) {
            var index = Array.IndexOf(this._inputActionNameOrder, actionName);
            return index >= 0 ? index : 0;
        }

        private static string GetButtonName(string buttonName, bool shortened) {
            var binding = _instance.GetActionBinding(_instance.GetActionIndex(buttonName));
            var path = binding.HasValue ? binding.Value.path : "";
            return shortened ? InputData.GetShortConversion(path) : InputData.GetLongConversion(path);
        }

        private void OnPrimaryButton(InputValue v) {
            PrimaryButton.SetPressed(v.isPressed);
        }

        private void OnSecondaryButton(InputValue v) {
            SecondaryButton.SetPressed(v.isPressed);
        }

        private void OnOptions(InputValue v) {
            OptionsButton.SetPressed(v.isPressed);
        }

        private void OnExitApplication(InputValue v) {
            ExitButton.SetPressed(v.isPressed);
        }

        private void OnLook(InputValue v) {
            LookDelta = (Vector2) v.Get();
        }
    }
}