using System;
using UnityEngine;

namespace Helpers {
    public static class TimeHelper {
        private static readonly DateTime StartDate = new DateTime(2020, 1, 1, 0, 0, 0);

        public static int GetDateTimeNowInHours() {
            return (int)(DateTime.Now - StartDate).TotalHours;
        }
    }
}