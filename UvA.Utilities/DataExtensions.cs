using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UvA.Utilities
{
    public static class DataExtensions
    {
        /// <summary>
        /// Converts a list of objects to a separated string
        /// </summary>
        /// <typeparam name="T">The type of the objects</typeparam>
        /// <param name="list">The list to convert</param>
        /// <param name="displayFunction">A function that gets converts each object to a string</param>
        /// <param name="separator">The separator to use between the objects</param>
        /// <param name="word">An optional word to use for the last object, e.g. 'and'</param>
        /// <returns></returns>
        public static string ToSeparatedString<T>(this IEnumerable<T> list, Func<T, string> displayFunction = null,
            string separator = ", ", string word = null)
        {
            // If no function is specified, just call the ToString method on each object
            if (displayFunction == null)
                displayFunction = d => d == null ? "" : d.ToString();

            StringBuilder builder = new StringBuilder();
            int count = 0;
            foreach (var l in list)
            {
                count++;

                // Append the object
                builder.Append(displayFunction(l));

                // If this is the last object, there is more than one object AND the word parameter is specified, 
                // add the word. Otherwise, add the separator if this is not the last object
                if (word != null && count > 0 && count == list.Count() - 1)
                    builder.Append(" " + word + " ");
                else if (count != list.Count())
                    builder.Append(separator);
            }
            return builder.ToString();
        }

        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var s in list)
                action(s);
        }
    }
}
