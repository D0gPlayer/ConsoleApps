using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApps.LeetCode
{ 
    //207. Course Schedule
    public partial class Solution
    {
		public bool CanFinish(int numCourses, int[][] prerequisites)
		{
			var prereqsMap = new Dictionary<int, List<int>>();
			for (int i = 0; i < prerequisites.Length; i++)
			{
				var course = prerequisites[i][0];
				var req = prerequisites[i][1];
				if(course == req) return false;
				if (prereqsMap.TryGetValue(course, out List<int> reqs)) reqs.Add(req);
				else prereqsMap[course] = new List<int> { req};
			}
			
			foreach (var course in prereqsMap)
			{
				if(!IsPrerequisiteValid(prereqsMap, course, new HashSet<int>())) return false;
				prereqsMap.Remove(course.Key);
			}
			
			return true;
		}
		
		private bool IsPrerequisiteValid(Dictionary<int, List<int>> prereqs, KeyValuePair<int, List<int>> course, HashSet<int> visited)
		{
			var isValid = false;
			visited.Add(course.Key);
			foreach (var req in course.Value)
			{
				if (!prereqs.ContainsKey(req)) isValid = true;
				else
				{
					isValid = !visited.Contains(req) && IsPrerequisiteValid(prereqs, prereqs.First(x => x.Key == req), visited);
					if(!isValid) break;
				}
			}
			if (isValid)
			{
				visited.Remove(course.Key);
				prereqs.Remove(course.Key);
			}
			return isValid;
		}
    }
}
