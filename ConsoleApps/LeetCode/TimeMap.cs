using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApps.LeetCode
{
    public partial class TimeMap
    {
        private Dictionary<string, Dictionary<int, string>> timeMap;
        public TimeMap()
        {
            timeMap = new Dictionary<string, Dictionary<int, string>>();
        }

        public void Set(string key, string value, int timestamp)
        {
            var keyExists = timeMap.TryGetValue(key, out Dictionary<int, string> values);
            if (keyExists) values[timestamp] = value;
            else
            {
                timeMap[key] = new Dictionary<int, string>();
                timeMap[key][timestamp] = value;
            }
        }

        public string Get(string key, int timestamp)
        {
            if (!timeMap.ContainsKey(key) || timeMap[key].First().Key > timestamp) return string.Empty;
            if (timeMap[key].TryGetValue(timestamp, out string result)) return result;

            var keys = timeMap[key].Keys;
            int left = 0, right = keys.Count - 1;

            while (left < right)
            {
                var mid = left + (right - left) / 2;

                if (keys.ElementAt(mid) > timestamp) right = mid - 1;
                else if (keys.ElementAt(mid) < keys.ElementAt(mid + 1) && keys.ElementAt(mid + 1) < timestamp) left = mid + 1;
                else return timeMap[key][keys.ElementAt(mid)];
            }
            return timeMap[key][keys.ElementAt(Math.Min(left, right))];
        }
    }
}
