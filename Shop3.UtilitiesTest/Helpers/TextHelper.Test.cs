using Shop3.Utilities.Helpers;
using Xunit;

namespace Shop3.UtilitiesTest.Helpers
{
    public class TextHelperTest
    {
        [Theory]
        [InlineData("noname 9x 3.2.1")]
        [InlineData("noname 9x- 3.2.1? ")]
        [InlineData("noname 9x-; - 3.2.1? ")]
        public void ToUnsignString_UpperCaseInput_LowerCaseOutput(string input)
        {
            var result = TextHelper.ToUnsignString(input);
            Assert.Equal("noname-9x-3-2-1", result);
        }

        [Fact]
        public void ToString_NumberInput_CharactersNumber()
        {
            var result = TextHelper.ToString(120);
            Assert.Equal("một trăm hai mươi đồng chẵn", result);
        }
    }
}
