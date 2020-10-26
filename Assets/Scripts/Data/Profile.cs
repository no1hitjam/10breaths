using System;
using System.Collections.Generic;

namespace Data {
    [Serializable]
    public class Profile {
        public List<Session> sessions;
        public int tutorialIndex;
        public int introductionIndex;

        public Session CurrentSession {
            get {
                if (this.sessions.Count == 0) {
                    this.sessions.Add(new Session());
                }

                return this.sessions[this.sessions.Count - 1];
            }
        }

        public Profile() {
            this.sessions = new List<Session>();
            this.tutorialIndex = 0;
            this.introductionIndex = 0;
        }
    }
}