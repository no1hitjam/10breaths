using System;
using System.Collections;
using BreathExercise;
using Data;
using Environment;
using Helpers;
using Input;
using TMPro;
using Ui;
using UnityEngine;
using UnityEngine.UI;


public class GameFlow : MonoBehaviour {
    private enum State {
        Startup,
        Title,
        TitleTransition,
        Session,
        SessionEnd,
    }

    [SerializeField] private Menu menu;
    [SerializeField] private BreathTool breathTool;
    [SerializeField] private Title title;
    [SerializeField] private EnvironmentManager environmentManager;
    [SerializeField] public CenterMenuButton centerMenuButton;

    private State _state = State.Startup;
    private bool _isOptionsButtonDown;

    private void Start() {
        if (DebugFlags.SkipTitleScreen) {
            this.StartFirstSessionNow();
        }
    }

    private void Update() {
        var optionsButtonIsDown = Inputs.OptionsButton.IsDown && !this._isOptionsButtonDown;
        this._isOptionsButtonDown = optionsButtonIsDown || Inputs.OptionsButton.IsDown;
        
        switch (this._state) {
            case State.Startup:
                if (Time.time > .2f) {
                    this.title.Show();
                    this._state = State.Title;
                }

                break;
            case State.Title:
                if (Inputs.PrimaryButton.IsHeldDown) {
                    this.StartCoroutine(this.StartFirstSessionFromTitle());
                } else if (Inputs.ExitButton.IsHeldDown) {
                    Application.Quit();
                }
                break;
            
            case State.TitleTransition:
                break;
            
            case State.Session:
                if (Inputs.OptionsButton.IsHeldDown) {
                    // Inp.Instance.MenuButton.Reset();
                    this.centerMenuButton.CompleteFill();
                    this.EndSession();
                }
                if (optionsButtonIsDown) {
                    this.breathTool.Center(false);
                }
                break;
            case State.SessionEnd:
                if (Inputs.PrimaryButton.IsHeldDown) {
                    this.menu.CompleteNewSession();
                    GameData.Instance.NewSession();
                    this.StartSession();
                } else if (Inputs.ExitButton.IsHeldDown) {
                    Application.Quit();
                }
                if (optionsButtonIsDown) {
                    this.menu.Center(false);
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private IEnumerator StartFirstSessionFromTitle() {
        this._state = State.TitleTransition;
        this.title.TransitionIntoSession();
        yield return new WaitForSeconds(2f);
        this.StartFirstSessionNow();
    }

    private void StartFirstSessionNow() {
        this.title.gameObject.SetActive(false);
        Inputs.PrimaryButton.SetPressed(false);
        this.environmentManager.StartDemo();
        this.StartSession();
    }

    private void StartSession() {
        Inputs.OptionsButton.SetPressed(false);
        this.menu.SetVisible(false);
        this.centerMenuButton.SetMenuFillActive(true);
        this.centerMenuButton.SetCenterActive(false);
        this.breathTool.Center(true);
        this.breathTool.SetActive(true);
        this._state = State.Session;
    }

    private void EndSession() {
        Inputs.OptionsButton.SetPressed(false);
        this.menu.SetVisible(true);
        this.centerMenuButton.SetMenuFillActive(false);
        this.breathTool.SetActive(false);
        this.centerMenuButton.SetCenterActive(true);
        this._state = State.SessionEnd;
    }
}
