using Helpers;
using UnityEngine;

namespace Environment {
    public readonly struct EnvironmentColor {
        public readonly Color skyColor1;
        public readonly Color skyColor2;
        public readonly Color skyColor3;
        public readonly Color skyColor4;
        public readonly Color skyColor5;
        public readonly float proportion;

        // Generated.
        public readonly Color GroundColor;
        public readonly Color Mountain1Color1;
        public readonly Color Mountain1Color2;
        public readonly Color Mountain2Color1;
        public readonly Color Mountain2Color2;
        public readonly Color Mountain3Color1;
        public readonly Color Mountain3Color2;

        public readonly Color StarMainColor;

        public readonly Color StarColor1;
        public readonly Color StarColor2;

        private static Color MaxAlpha(Color c) {
            c.a = 1;
            return c;
        }

        public EnvironmentColor(Color skyColor1, Color skyColor2, Color skyColor3, Color skyColor4, Color skyColor5,
            float proportion) {
            this.skyColor1 = skyColor1;
            this.skyColor2 = skyColor2;
            this.skyColor3 = skyColor3;
            this.skyColor4 = skyColor4;
            this.skyColor5 = skyColor5;
            this.proportion = proportion;
            this.GroundColor = MaxAlpha(skyColor1 * .6f);
            // Mountain 1 is furthest. Color 1 is back, 2 is front. 
            this.Mountain1Color1 =
                MaxAlpha(new Color(this.skyColor1.r * .8f, this.skyColor1.g * .9f, this.skyColor1.b * .9f));

            this.Mountain1Color2 = MaxAlpha(Color.Lerp(this.skyColor2, this.skyColor5, .5f));
            this.Mountain2Color1 = MaxAlpha(Color.Lerp(this.Mountain1Color1, this.skyColor2, .1f));
            this.Mountain2Color2 = MaxAlpha(Color.Lerp(this.Mountain1Color2, Color.Lerp(this.Mountain1Color1, this.skyColor2, .3f), .5f));
            this.Mountain3Color1 = MaxAlpha(Color.Lerp(this.Mountain2Color1, this.skyColor3, .1f));
            this.Mountain3Color2 = MaxAlpha(Color.Lerp(this.Mountain1Color1, this.skyColor2, .3f));

            var skyColor1Brightness = this.skyColor1.Brightness();
            const float blueStarDivision = .08f;
            var blueStarColor = new Color(.1f, .35f, .9f);
            var lightStarColor = this.skyColor5 + new Color(.1f, .15f, .2f, 0);
            if (skyColor1Brightness < blueStarDivision) {
                this.StarMainColor = Color.Lerp(
                    blueStarColor,
                    lightStarColor,
                    (skyColor1Brightness) / (blueStarDivision));
            } else {
                this.StarMainColor = Color.Lerp(
                    lightStarColor,
                    this.skyColor5,
                    (skyColor1Brightness - blueStarDivision) / (1f - blueStarDivision));
            }

            this.StarColor1 = Color.Lerp(this.skyColor2, this.StarMainColor, .5f + this.skyColor2.Brightness() * .5f);
            this.StarColor2 = Color.Lerp(this.skyColor3, 
                Color.Lerp(blueStarColor, Color.white, skyColor1Brightness),
                .3f + (1 - this.skyColor3.Brightness()) * .7f);
        }

        public EnvironmentColor(Color skyColor1, Color skyColor2,
            float proportion) : this(
            skyColor1,
            Color.Lerp(skyColor1, skyColor2, .25f),
            Color.Lerp(skyColor1, skyColor2, .5f),
            Color.Lerp(skyColor1, skyColor2, .75f),
            skyColor2, proportion) {
        }

        public EnvironmentColor(Color color, float proportion) : this(color, color, color, color, color, proportion) {
        }

        public static EnvironmentColor Lerp(EnvironmentColor a, EnvironmentColor b, float amount) {
            return new EnvironmentColor(
                Color.Lerp(a.skyColor1, b.skyColor1, amount),
                Color.Lerp(a.skyColor2, b.skyColor2, amount),
                Color.Lerp(a.skyColor3, b.skyColor3, amount),
                Color.Lerp(a.skyColor4, b.skyColor4, amount),
                Color.Lerp(a.skyColor5, b.skyColor5, amount),
                Mathf.Lerp(a.proportion, b.proportion, amount)
            );
        }
    }
}