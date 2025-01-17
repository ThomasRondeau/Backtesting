using System;
using System.Collections.Generic;

namespace Backtesting.Indicators
{
    public class IndicatorCache<T>
    {
        private Queue<T> cache;
        private int maxSize;

        public IndicatorCache(int maxSize)
        {
            this.maxSize = maxSize;
            cache = new Queue<T>(maxSize);
        }

        public void Add(T value)
        {
            if (cache.Count >= maxSize)
            {
                cache.Dequeue();
            }
            cache.Enqueue(value);
        }

        public T[] GetAll()
        {
            return cache.ToArray();
        }

        public void Clear()
        {
            cache.Clear();
        }
    }
}
