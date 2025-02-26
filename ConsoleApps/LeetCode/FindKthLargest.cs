using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApps.LeetCode
{
    //215. Kth Largest Element in an Array 
    public partial class Solution
    {
        public int FindKthLargest(int[] nums, int k)
        {
            var heap = new Heap(k);
            for (int i = 0; i < nums.Length; i++)
            {
                heap.Add(nums[i]);
            }

            return heap.Peek();
        }
        
        private class Heap
        {
            private List<int> heap;
            private int capacity;
            
            public Heap(int _capacity)
            {
                heap = new List<int>();
                capacity = _capacity;
            }
            
            public void Add(int val)
            {
                if (heap.Count == 0)
                {
                    heap.Add(val);
                    return;
                }
                else if(val <= heap[0]  && heap.Count == capacity) return;

                int left = 0, right = heap.Count, mid = 0;
                while (left <= right)
                {
                    mid = left + (right - left) / 2;
                    
                    if(mid == heap.Count) break;
                    else if (heap[mid] < val) left = mid + 1;
                    else if (heap[mid] > val) right = mid - 1;
                    else
                    {
                        break;
                    }
                }
                heap.Insert(Math.Max(mid, left), val);
                
                if(heap.Count > capacity) heap.RemoveAt(0);
            }
            
            public int Peek()
            {
                return heap[0];
            }
        }
    }
}
