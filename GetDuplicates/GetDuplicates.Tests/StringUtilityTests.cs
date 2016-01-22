using GetDuplicates.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GetDuplicates.Tests
{
    public class StringUtilityTests
    {
        public class GetDuplicateElementsTests
        {
            [Fact(DisplayName = "Throws on null argument")]
            public void ThrowsOnNullArgument()
            {
                var exception = Assert.Throws<ArgumentNullException>("value", () => StringUtility.GetDuplicateElements(null));
            }

            [Fact(DisplayName = "Returns read-only collection of strings")]
            public void ReturnsReadonlyCollectionOfStrings()
            {
                var actual = StringUtility.GetDuplicateElements(string.Empty);

                Assert.IsAssignableFrom<IReadOnlyCollection<string>>(actual);
            }

            [Fact(DisplayName = "Returns no duplicates for empty string")]
            public void NoDuplicatesInEmptyString()
            {
                var actual = StringUtility.GetDuplicateElements(string.Empty);

                Assert.IsAssignableFrom<IReadOnlyCollection<string>>(actual);

                Assert.Empty(actual);
            }

            [Fact(DisplayName = "Should handle culture-specific casing correctly")]
            [UseCulture("TR-tr")]
            public void ShouldHandleCultureSpecificCasing()
            {
                var actual = StringUtility.GetDuplicateElements("İİii").Count;

                Assert.Equal(1, actual);
            }

            [Theory(DisplayName = "Returns no duplicates for strings with unique elements")]
            [InlineData("qwerty", 0)]
            [InlineData("12345", 0)]
            [InlineData("1234567890qwertyuiop[]';lkjhgfdsazxcvbnm,./?><:\"}{+_)(*&^%$#@!~", 0)]
            [InlineData("\n\t", 0)]
            [InlineData("ldkf 0432", 0)]
            public void NoDuplicatesForUniqueStrings(string input, int expected)
            {
                var actual = StringUtility.GetDuplicateElements(input).Count;

                Assert.Equal(expected, actual);
            }

            [Theory(DisplayName = "Is case-insensitive")]
            [InlineData("qQ", 1)]
            [InlineData("aBBBbbacC", 3)]
            [InlineData("aBcD", 0)]
            [InlineData("İİiiı", 2)]
            [UseCulture("EN-us")]
            public void IsCaseInSensitive(string input, int expected)
            {
                var actual = StringUtility.GetDuplicateElements(input).Count;

                Assert.Equal(expected, actual);
            }

            [Theory(DisplayName = "Returns correct number of duplicates when there are any")]
            [InlineData("qqqqq", 1)]
            [InlineData("123123123", 3)]
            [InlineData("ldkf 0432     ", 1)]
            [InlineData("a𤭢s𤭢אֳאֳgryאֳmn8", 2)]
            [InlineData("e\u0301e\u0301e\u0301İİ", 2)]
            public void ReturnsCorrectNumberOfDuplicatesWhenThereAreAny(string input, int expected)
            {
                var actual = StringUtility.GetDuplicateElements(input).Count;

                Assert.Equal(expected, actual);
            }

            [Theory(DisplayName = "Returned duplicates are from the input string only")]
            [InlineData("qqqqq")]
            [InlineData("qqqQQ")]
            [InlineData("123123123")]
            [InlineData("ldkf 0432     ")]
            public void ReturnsDuplicatesFromInputOnly(string input)
            {
                var actual = StringUtility.GetDuplicateElements(input);

                Assert.All(actual, element => Assert.Contains(element, input, StringComparison.CurrentCultureIgnoreCase));
            }

            [Theory(DisplayName = "Returns unique list of elements")]
            [InlineData("qQ")]
            [InlineData("iİİiiIIıı")]
            [InlineData("qqqqq")]
            [InlineData("123123123")]
            [InlineData("aBBBbbacC")]
            public void ReturnsUniqueListOfElements(string input)
            {
                var actual = StringUtility.GetDuplicateElements(input);

                Assert.Equal(actual.Distinct(StringComparer.CurrentCultureIgnoreCase), actual);
            }
        }
    }
}
