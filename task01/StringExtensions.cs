using System;
using System.Linq;

namespace task01
{
    public static class StringExtensions
    {
        public static bool IsPalindrome(this string input)
        {
            if (input == "")
            {
                return false;
            }

            var NormChars = input.ToLower().Where(c => !char.IsPunctuation(c) && !char.IsWhiteSpace(c)).ToArray();

            if (NormChars.Length < 2)
            {
                return false;
            }

            int left = 0;
            int right = NormChars.Length - 1;

            while (left < right)
            {
                if (NormChars[left] != NormChars[right])
                {
                    return false;
                }
                left++;
                right--;
            }

            return true;
        }
    }
}