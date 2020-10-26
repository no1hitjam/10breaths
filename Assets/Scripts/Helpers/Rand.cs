using System;
using Structures;

namespace Helpers {
    public static class Rand {
        private static readonly System.Random SysRandom = new Random();
        public static float Value => (float) SysRandom.NextDouble();

        public static float Range(float min, float max) => UnityEngine.Random.Range(min, max);
        public static int Range(int min, int max) => UnityEngine.Random.Range(min, max);

        public static int Sign => Bool ? 1 : -1;
        public static bool Bool => Value > .5f;

        public static T Element<T>(NonEmptyArray<T> array) {
            return array.Values[Range(0, array.Length)];
        }

        public static int WeightedIndex<T>(WeightedArray<T> array) {
            var total = 0f;
            foreach (var element in array.Values) {
                total += element.Item2;
            }

            var randomValue = Range(0f, total);
            for (var i = 0; i < array.Values.Length; i++) {
                var (item1, item2) = array.Values[i];
                randomValue -= item2;
                if (randomValue <= 0) {
                    return i;
                }
            }

            return 0;
        }

        public static T WeightedElement<T>(WeightedArray<T> array) {
            var total = 0f;
            foreach (var element in array.Values) {
                total += element.Item2;
            }

            var randomValue = Range(0f, total);
            foreach (var (item1, item2) in array.Values) {
                randomValue -= item2;
                if (randomValue <= 0) {
                    return item1;
                }
            }

            return array.Values[0].Item1;
        }
    }
}