﻿using System.Collections.Generic;

namespace TecX.Common.Comparison
{
    public class DictionaryComparer<TKey, TValue> : EqualityComparer<IDictionary<TKey, TValue>>
    {
        public override bool Equals(IDictionary<TKey, TValue> x, IDictionary<TKey, TValue> y)
        {
            if (Compare.AreBothNull(x, y))
                return true;

            if (Compare.IsExactlyOneNull(x, y))
                return false;

            if (x.Count != y.Count)
                return false;

            foreach (TKey key in x.Keys)
            {
                //one dictionary contains elements the other does not contain -> not equal
                if (x.ContainsKey(key) && y.ContainsKey(key))
                {
                    TValue valueFirst = x[key];
                    TValue valueScnd = y[key];

                    if (!Compare.AreEqual(valueFirst, valueScnd))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode(IDictionary<TKey, TValue> obj)
        {
            Guard.AssertNotNull(obj, "obj");

            int hash = obj.GetHashCode();

            return hash;
        }
    }
}