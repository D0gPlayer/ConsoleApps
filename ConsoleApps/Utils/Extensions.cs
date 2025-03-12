using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApps.Utils
{
    public class ListNode
    {
        public int val;
        public ListNode next;

        public ListNode(int val)
        {
            this.val = val;
        }

        public static ListNode FromArray(int[] arr)
        {
            if (arr == null || arr.Length == 0)
            {
                return null; // Return null for empty array
            }

            ListNode head = new ListNode(arr[0]); // Create the head node
            ListNode current = head; // Use current to build the list

            for (int i = 1; i < arr.Length; i++)
            {
                current.next = new ListNode(arr[i]); // Create new node for each element
                current = current.next; // Move to the next node
            }

            return head; // Return the head of the linked list
        }
    }

    public class TreeNode
    {
        public int? val;
        public TreeNode left;
        public TreeNode right;
        public TreeNode(int? val = 0, TreeNode left = null, TreeNode right = null)
        {
            this.val = val;
            this.left = left;
            this.right = right;
        }

        public static TreeNode FromList(List<int?> list)
        {
            if (list.Count == 0) return null;

            TreeNode root = new TreeNode((int?)list[0]);
            Queue<TreeNode> queue = new Queue<TreeNode>();
            queue.Enqueue(root);

            int i = 1;

            while (i < list.Count)
            {
                TreeNode current = queue.Dequeue();

                // Create left child
                if (i < list.Count && list[i] != null)
                {
                    current.left = new TreeNode(list[i]);
                    queue.Enqueue(current.left);
                }
                i++;

                // Create right child
                if (i < list.Count && list[i] != null)
                {
                    current.right = new TreeNode(list[i]);
                    queue.Enqueue(current.right);
                }
                i++;
            }

            return root;
        }
    }
}
