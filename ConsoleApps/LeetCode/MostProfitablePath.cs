using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApps.LeetCode
{
    public partial class Solution
    {
        public int MostProfitablePath(int[][] edges, int bob, int[] amount)
        {
            //every node has cost/reward
            //bob goes from node bob to 0, collecting costs/rewards from nodes
            //if t on node = bobTime - half the cost/rewards, if t < bobTime - full, if t > bobTime - 0
            //we start from zero and need to find the most profitable path
            var n = amount.Length;
            var tree = new List<int>[n];

            //Step 1. Building adjacency list for nodes from edges
            for (int i = 0; i < n; i++)
            {
                tree[i] = new List<int>();
            }
            for (int i = 0; i < edges.Length; i++)
            {
                int x = edges[i][0], y = edges[i][1];
                tree[x].Add(y);
                tree[y].Add(x);
            }

            //Step 2. Building parent[] for nodes to use DFS
            var parents = new int[n];
            Array.Fill(parents, -1);
            DFSParent(0, -1, tree, parents);

            //Step 3. Filling bobTime
            var bobTime = new int[n];
            Array.Fill(bobTime, int.MaxValue);
            var time = 0;
            var node = bob;
            while (node != -1)
            {
                bobTime[node] = time;
                time++;
                node = parents[node];
            }

            //Step 4. Find the most profitable leaf with DFS
            return DFSProfit(0, -1, tree, parents, bobTime, amount);
        }

        private void DFSParent(int node, int parent, List<int>[] neighbors, int[] parents)
        {
            parents[node] = parent;
            foreach (var neighbor in neighbors[node])
            {
                if (parent != neighbor) DFSParent(neighbor, node, neighbors, parents);
            }
        }

        private int DFSProfit(int node, int time, List<int>[] neighbors, int[] parents, int[] bobTime, int[] profits)
        {
            time++;
            var profit = 0;
            if (time < bobTime[node]) profit += profits[node];
            else if (time == bobTime[node]) profit += profits[node] / 2;

            var neighborProfits = new SortedSet<int>();
            foreach (var neighbor in neighbors[node])
            {
                if (neighbor != parents[node])
                {
                    neighborProfits.Add(DFSProfit(neighbor, time, neighbors, parents, bobTime, profits));
                }
            }

            return profit + neighborProfits.Max;
        }
    }
}
