using System.Linq;
using Helpers;
using UnityEngine;

namespace Data {
    public readonly struct Stats {
        public readonly int Most10BreathsInASession;
        public readonly int TotalFull10BreathCount;
        public readonly int LatestDailyStreak;

        public Stats(Profile profile) {
            this.Most10BreathsInASession = F.Max(F.Map(profile.sessions, s => s.full10BreathCount));
            this.TotalFull10BreathCount = profile.sessions.Sum(session => session.full10BreathCount);

            var latestSessionStartTime = profile.sessions[profile.sessions.Count - 1].startTime;
            var runningSessionTime = latestSessionStartTime;
            var totalSessionsInStreak = 1;
            for (var i = profile.sessions.Count - 2; i >= 0; i--) {
                var sessionStartTime = profile.sessions[i].startTime;
                totalSessionsInStreak++;
                if (runningSessionTime - sessionStartTime > 48) {
                    break;
                }

                runningSessionTime = sessionStartTime;
            }

            this.LatestDailyStreak =
                Mathf.Max(1,
                    Mathf.Min(totalSessionsInStreak,
                        Mathf.FloorToInt((latestSessionStartTime + 24 + 6 - runningSessionTime) / (float) 24)));
        }
    }
}