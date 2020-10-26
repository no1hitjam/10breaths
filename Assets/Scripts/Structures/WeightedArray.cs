using UnityEngine.Assertions;

namespace Structures {
    public class WeightedArray<T> {
        public readonly (T, float)[] Values;

        public WeightedArray((T, float)[] values) {
            this.Values = values;
            Assert.IsTrue(this.Values.Length > 0);
        }

        public WeightedArray(T value) {
            this.Values = new[] {(value, 1f)};
        }
    }
}