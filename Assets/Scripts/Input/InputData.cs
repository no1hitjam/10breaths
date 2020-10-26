using System.Collections.Generic;
using System.Numerics;
using JetBrains.Annotations;

namespace Input {
    public static class InputData {
        private readonly struct ButtonNameConversion {
            public readonly string ShortName;
            public readonly string LongName;

            public ButtonNameConversion(string shortName, string longName) {
                this.ShortName = shortName;
                this.LongName = longName;
            }
        }

        private static readonly Dictionary<string, ButtonNameConversion> ButtonNameConversions =
            new Dictionary<string, ButtonNameConversion> {
                {"<Mouse>/leftButton", new ButtonNameConversion("Left Mouse", "the Left Mouse Button")},
                {"<Mouse>/rightButton", new ButtonNameConversion("Right Mouse", "the Right Mouse Button")},
                {"<Keyboard>/escape", new ButtonNameConversion("Esc", "the 'Esc' key")},
                {"<Keyboard>/space", new ButtonNameConversion("Space", "the 'Space' key")},
            };

        [NotNull]
        public static string GetShortConversion(string key) {
            return ButtonNameConversions.ContainsKey(key) ? ButtonNameConversions[key].ShortName : key;
        }
        
        [NotNull]
        public static string GetLongConversion(string key) {
            return ButtonNameConversions.ContainsKey(key) ? ButtonNameConversions[key].LongName : key;
        }
    }
}