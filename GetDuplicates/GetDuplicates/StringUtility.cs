using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDuplicates
{
    public static class StringUtility
    {
        /// <summary>
        /// Returns a read-only collection of duplicate elements in the string ignoring their case
        /// and using the word comparison rules of the current culture.
        /// The casing of returned elements corresponds to their first occurence in the input string,
        /// e.g. 'aA' will return ['a'] and 'Aa' will return ['A'].
        /// </summary>
        /// <param name="value">String to analyze for duplicates</param>
        /// <returns>A read-only collection of duplicate elements</returns>
        public static IReadOnlyCollection<string> GetDuplicateElements(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var enumerator = System.Globalization.StringInfo.GetTextElementEnumerator(value);
            // A dictionary to store string elements we find.
            // If a key returns true, it means that this string element is duplicate.
            // While using Ordinal culture is considerably faster, it is not
            // culturally-agnostic and as stated in the guidelines:
            //
            // https://msdn.microsoft.com/en-us/library/ms973919.aspx
            //
            // whenever user input is involved, it is recommended to use CurrentCulture* comparison rules
            var characters = new Dictionary<string, bool>(StringComparer.CurrentCultureIgnoreCase);

            // Alternatively, we could use a list of string elements like that, for example:
            //
            // var stringInfo = new System.Globalization.StringInfo(s);
            // var enumerator = System.Globalization.StringInfo.GetTextElementEnumerator(s);
            // var ret = new List<string>();
            // var index = 0;
            // while (enumerator.MoveNext())
            // {
            //     index++;
            //     var currentStr = enumerator.Current.ToString();
            //
            //     if (ret.Contains(currentStr, StringComparer.CurrentCultureIgnoreCase) == false
            //         && enumerator.ElementIndex > 0
            //         && stringInfo.SubstringByTextElements(0, index).IndexOf(currentStr, StringComparison.CurrentCultureIgnoreCase) >= 0)
            //     {
            //         ret.Add(currentStr);
            //     }
            // }
            //
            // return ret.AsReadOnly();
            //
            // but it would only be more efficient than dictionary
            // on strings with small number of duplicates (3-4). This can be explained by the fact that Dictionary<,>.TryGetValue()
            // is an O(1) operation and List<>.Contains() (and any other lookup) is an O(n) operation.

            while (enumerator.MoveNext())
            {
                var currentElement = enumerator.Current.ToString();
                var isDuplicateElement = false;

                // TryGetValue is slightly faster than ContainsKey
                if (characters.TryGetValue(currentElement, out isDuplicateElement))
                {
                    // we've encountered this element earlier
                    // let's check if that's the second time we see it
                    if (isDuplicateElement == false)
                    {
                        characters[currentElement] = true;
                    }
                }
                else // we haven't encountered this element before
                {
                    characters.Add(currentElement, false);
                }
            }

            return characters.Where(kvp => kvp.Value).Select(kvp => kvp.Key).ToList().AsReadOnly();
        }
    }
}
