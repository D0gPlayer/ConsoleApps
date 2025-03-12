using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApps.LeetCode
{
    public class LRUCache
    {
        private int _maxCapacity;
        private OrderedDictionary<int, int> _cache;

        public LRUCache(int capacity)
        {
            _maxCapacity = capacity;
            _cache = new OrderedDictionary<int, int>(capacity);
        }

        public int Get(int key)
        {
            if (_cache.TryGetValue(key, out int value))
            {
                _cache.Remove(key);
                _cache.Add(key, value);
                return value;
            }
            else return -1;
        }

        public void Put(int key, int value)
        {
            if (_cache.ContainsKey(key))
            {
                _cache.Remove(key);
                _cache.Add(key, value);
                return;
            }
            else if (_cache.Count == _maxCapacity) _cache.RemoveAt(0);

            _cache.Add(key, value);
        }
    }
}
