using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApps.LeetCode
{
    public partial class Solution
    {
        public long CountOfSubstrings(string word, int k)
        {
            int[] isVowel = new int[128]; // To mark vowels
            int[] freq = new int[128]; // To track character frequency
            string vowels = "aeiou";

            // Mark vowels in the isVowel array
            foreach (char v in vowels)
            {
                isVowel[v] = 1;
            }

            long response = 0;
            int currentK = 0, vowelCount = 0, extraLeft = 0, left = 0;

            for (int right = 0; right < word.Length; right++)
            {
                char rightChar = word[right];

                if (isVowel[rightChar] == 1)
                {
                    if (++freq[rightChar] == 1) vowelCount++;
                }
                else
                {
                    currentK++;
                }

                // Shrink window if consonant count exceeds k
                while (currentK > k)
                {
                    char leftChar = word[left];
                    if (isVowel[leftChar] == 1)
                    {
                        if (--freq[leftChar] == 0) vowelCount--;
                    }
                    else
                    {
                        currentK--;
                    }
                    left++;
                    extraLeft = 0;
                }

                // Adjust left pointer to remove extra vowels
                while (vowelCount == 5 && currentK == k && left < right && isVowel[word[left]] == 1 && freq[word[left]] > 1)
                {
                    extraLeft++;
                    freq[word[left]]--;
                    left++;
                }

                // Count valid substrings
                if (currentK == k && vowelCount == 5)
                {
                    response += (1 + extraLeft);
                }
            }

            return response;
        }
    }
}
