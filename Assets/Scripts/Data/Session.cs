using System;
using Helpers;
using UnityEngine;

namespace Data {
    [Serializable]
    public class Session {
        public int startTime;
        public int full10BreathCount;

        public Session(int? debugRelativeStartHours = null) {
            var now = TimeHelper.GetDateTimeNowInHours();
            this.startTime = (now + debugRelativeStartHours) ?? now;
            this.full10BreathCount = 0;
        }
    }
}