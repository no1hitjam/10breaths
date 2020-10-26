using Data;
using UnityEngine.Assertions;

namespace Tests {
    public class StreakTests {
        [NUnit.Framework.Test]
        public void SingleSessionHas1DayStreak() {
            var profile = new Profile();
            profile.sessions.Add(new Session());
            var stats = new Stats(profile);
            Assert.AreEqual(1, stats.LatestDailyStreak);
        }
        
        [NUnit.Framework.Test]
        public void TwoSessionsUnderHalfADayApartWith1DayStreak() {
            var profile = new Profile();
            profile.sessions.Add(new Session(0));
            profile.sessions.Add(new Session(18 - 1));
            var stats = new Stats(profile);
            Assert.AreEqual(1, stats.LatestDailyStreak);
        }
        
        [NUnit.Framework.Test]
        public void TwoSessionsADayApartWith2DayStreaks() {
            var profile = new Profile();
            profile.sessions.Add(new Session(0));
            profile.sessions.Add(new Session(24));
            var stats = new Stats(profile);
            Assert.AreEqual(2, stats.LatestDailyStreak);
        }
        
        [NUnit.Framework.Test]
        public void TwoSessionsJustUnderTwoDaysApartWith2DayStreaks() {
            var profile = new Profile();
            profile.sessions.Add(new Session(0));
            profile.sessions.Add(new Session(48 - 1));
            var stats = new Stats(profile);
            Assert.AreEqual(2, stats.LatestDailyStreak);
        }
        
        [NUnit.Framework.Test]
        public void TwoSessionsJustOverTwoDaysApartWithout2DayStreaks() {
            var profile = new Profile();
            profile.sessions.Add(new Session(0));
            profile.sessions.Add(new Session(48 + 1));
            var stats = new Stats(profile);
            Assert.AreEqual(1, stats.LatestDailyStreak);
        }
        
        [NUnit.Framework.Test]
        public void ThreeSessionsADayApartWith3DayStreaks() {
            var profile = new Profile();
            profile.sessions.Add(new Session(0));
            profile.sessions.Add(new Session(24));
            profile.sessions.Add(new Session(48));
            var stats = new Stats(profile);
            Assert.AreEqual(3, stats.LatestDailyStreak);
        }
        
        [NUnit.Framework.Test]
        public void ThreeSessionsJustUnderTwoDaysApartWith3DayStreaks() {
            var profile = new Profile();
            profile.sessions.Add(new Session(0));
            profile.sessions.Add(new Session(48 - 1));
            profile.sessions.Add(new Session(72 - 2));
            var stats = new Stats(profile);
            Assert.AreEqual(3, stats.LatestDailyStreak);
        }
        
        [NUnit.Framework.Test]
        public void ThreeSessionsJustOverTwoDaysApartWithout3DayStreaks() {
            var profile = new Profile();
            profile.sessions.Add(new Session(0));
            profile.sessions.Add(new Session(24 - 1));
            profile.sessions.Add(new Session(72));
            var stats = new Stats(profile);
            Assert.AreEqual(1, stats.LatestDailyStreak);
        }
    }
}