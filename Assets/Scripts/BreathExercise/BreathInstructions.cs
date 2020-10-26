using System.Collections.Generic;
using TMPro;
using Tween;
using Ui;
using UnityEngine;

namespace BreathExercise {
    public static class BreathInstructions {
        private const string StartBreathingIn = "Now, at the start of each in-breath, hold <Primary Button>";
        private const string StartBreathingOut = "and release it at the start of each out-breath.";

        private const string StartBreathingIn2 = "Take your time";
        private const string StartBreathingOut2 = "and try not to strain your breaths.";
        private const string StartBreathingOut3 = "Now, hold <Secondary Button> to finish the 10 breath cycle.";

        public const string Summary = "<b>10 Breath Cycle</b> instructions:\n" +
                                       "• Hold <Primary Button> during your in-breath. Release on the out-breath.\n" +
                                       "• Count 10 breaths, then hold <Secondary Button> to complete a cycle.\n" + 
                                       "• When finished, hold <Options> to end the session, or <Exit Application> to exit the application.\n";

        public static string[] Get(int tutorialIndex, int breathCount, bool breatheIn) {
            string textString1 = null;
            string textString2 = null;
            if (tutorialIndex == 0) {
                switch (breathCount) {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                        textString1 = StartBreathingIn;
                        textString2 = breathCount > 0 || !breatheIn ? StartBreathingOut : null;
                        break;
                    case 6:
                    case 7:
                        textString1 = StartBreathingIn2;
                        textString2 = breathCount > 6 || !breatheIn ? StartBreathingOut2 : null;
                        break;
                    case 10:
                        textString1 = StartBreathingOut3;
                        break;
                }
            }

            return new[] {textString1, textString2};
        }
    }
}