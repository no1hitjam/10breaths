using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Structures {
    public readonly struct NonEmptyArray<T> {
        public readonly T[] Values;

        public int Length => this.Values.Length;

        public NonEmptyArray(T[] values) {
            Assert.IsTrue(values.Length > 0);
            this.Values = values;
        }

        public static NonEmptyArray<T>? From(T[] value) {
            if (value.Length == 0) {
                return null;
            }

            return new NonEmptyArray<T>(value);
        }

        public static NonEmptyArray<T>? From(List<T> value) {
            return From(value.ToArray());
        }
    }
}
