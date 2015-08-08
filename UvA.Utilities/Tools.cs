using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UvA.Utilities
{
    public static class Tools
    {
        /// <summary>
        /// Generates an array containing all numbers in a specific range
        /// </summary>
        /// <param name="min">Start of range</param>
        /// <param name="max">End of range</param>
        public static int[] Range(int min, int max)
        {
            List<int> result = new List<int>();
            for (int i = min; i <= max; i++)
                result.Add(i);
            return result.ToArray();
        }
    }
}
