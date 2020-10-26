using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Helpers {
    /// <summary>
    /// Functional functions 
    /// </summary>
    public static class F {
        public static IEnumerable<T2> Map<T1, T2>(IEnumerable<T1> items, Func<T1, T2> f) {
            foreach (var item in items) {
                yield return f(item);
            }
        }

        public static T Max<T>(IEnumerable<T> items)
            where T : struct, IComparable {
            return Compare(items, (a, b) => a.CompareTo(b) > 0);
        }
        
        public static T Min<T>(IEnumerable<T> items)
            where T : struct, IComparable {
            return Compare(items, (a, b) => a.CompareTo(b) < 0);
        }

        private static T Compare<T>(IEnumerable<T> items, Func<T, T, bool> compareF) 
            where T : struct, IComparable {
            T? max = null;
            foreach (var item in items) {
                if (!max.HasValue || compareF(item, max.Value)) {
                    max = item;
                }
            }

            return max ?? default(T);
        }

        public static TClass Max<TCompare, TClass>(IEnumerable<TClass> items, Func<TClass, TCompare> f)
            where TClass : class
            where TCompare : struct, IComparable  {
            return Compare(items, f, (a, b) => a.CompareTo(b) > 0);
        }
        
        private static TClass Min<TCompare, TClass>(IEnumerable<TClass> items, Func<TClass, TCompare> f)
            where TClass : class
            where TCompare : struct, IComparable  {
            return Compare(items, f, (a, b) => a.CompareTo(b) < 0);
        }

        [CanBeNull]
        private static TClass Compare<TCompare, TClass>(IEnumerable<TClass> items, Func<TClass, TCompare> f,
                Func<IComparable, IComparable, bool> compareF)
            where TClass : class
            where TCompare : struct, IComparable {
            TCompare? maxAmount = null;
            TClass result = null;
            foreach (var item in items) {
                if (item == null) {
                    continue;
                }

                var itemAmount = f(item);
                if (!maxAmount.HasValue || compareF(itemAmount, maxAmount.Value)) {
                    maxAmount = itemAmount;
                    result = item;
                }
            }

            return result;
        }
    }
}