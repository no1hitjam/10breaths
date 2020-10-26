using System;
using System.Collections.Generic;
using System.Linq;
using SaveData;
using UnityEngine;

namespace Data {
    public class GameData {
        private int _profileIndex;
        private readonly List<Profile> _profiles;

        private static GameData _instance;

        public static GameData Instance {
            get {
                if (_instance == null) {
                    _instance = new GameData();
                }

                return _instance;
            }
        }

        private GameData() {
            // Load data from files.
            SaveLoadFunctions.Load(out this._profiles);

            this._profileIndex = 0;
            this._profileIndex = PlayerPrefs.GetInt(SaveLoadConstants.ProfileIndexKey, 0);
            if (this._profileIndex > this._profiles.Count) {
                this._profileIndex = 0;
            }
            
            if (DebugFlags.StartWith4DayStreak) {
                var debugProfile = new Profile();
                debugProfile.sessions.Add(new Session(-24 * 3));
                debugProfile.sessions.Add(new Session(-24 * 2));
                debugProfile.sessions.Add(new Session(-24));
                debugProfile.introductionIndex = 4;
                debugProfile.tutorialIndex = 1;
                this._profiles = new List<Profile> { debugProfile };
                this._profileIndex = 0;
            }
            
            this.NewSession();
            this.Save();
            this.IsFirstSessionAfterOpening = true;
        }

        public void NewSession() {
            this.CurrentProfile.sessions.Add(new Session());
            this.Save();
            this.IsFirstSessionAfterOpening = false;
        }

        public void Increment10BreathCount() {
            this.CurrentProfile.CurrentSession.full10BreathCount++;
            this.Save();
        }

        public Stats CurrentStats => new Stats(this.CurrentProfile);

        private Profile CurrentProfile => this._profiles[this._profileIndex];
        public int TimeInSession => DateTime.Now.Minute - this.CurrentProfile.CurrentSession.startTime;
        public int Current10BreathCount => this.CurrentProfile.CurrentSession.full10BreathCount;

        public int TutorialIndex {
            get => this.CurrentProfile.tutorialIndex;
            set { 
                this.CurrentProfile.tutorialIndex = value;
                this.Save();
            }
        }

        public int IntroductionIndex {
            get => this.CurrentProfile.introductionIndex;
            set {
                this.CurrentProfile.introductionIndex = value;
                this.Save();
            }
        }

        public bool IsFirstSessionAfterOpening { get; private set; }

        private void Save() {
            SaveLoadFunctions.SaveFile(SaveLoadConstants.ProfileSaveFilename, this._profiles);
        }
    }
}