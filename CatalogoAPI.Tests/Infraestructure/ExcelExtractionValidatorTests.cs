using System;
using System.Threading.Tasks;
using Catalog.Infrastructure.Extraction;
using Xunit;

namespace Catalog.Tests.Infraestructure
{
    public class ExcelExtractionValidatorTests
    {
        private readonly ExcelExtractionValidator validator;

        public ExcelExtractionValidatorTests()
        {
            validator = new ExcelExtractionValidator();
        }

        [Fact]
        public void File_is_empty()
        {
            validator.NotifyEmptySpreadsheet();

            var isValid = validator.IsExtractionValid();
            Assert.False(isValid);
        }

        [Fact]
        public void File_is_valid()
        {
            var isValid = validator.IsExtractionValid();
            Assert.True(isValid);
        }

        [Fact]
        public void File_has_valid_Int_field()
        {
            object fieldValue = 1;

            var value = validator.TryGetInt("Teste", fieldValue, 1, 1);

            var isValid = validator.IsExtractionValid();

            Assert.Equal(value, fieldValue);
            Assert.True(isValid);
        }

        [Fact]
        public void File_has_not_valid_int_field()
        {
            object fieldValue = "";

            var value = validator.TryGetInt("Teste", fieldValue, 1, 1);

            var isValid = validator.IsExtractionValid();

            Assert.NotEqual(value, fieldValue);
            Assert.False(isValid);
        }

        [Fact]
        public void File_has_valid_string_field()
        {
            object fieldValue = "asdfg";

            var value = validator.TryGetString("Teste", fieldValue, 1, 1);

            var isValid = validator.IsExtractionValid();

            Assert.Equal(value, fieldValue);
            Assert.True(isValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void File_has_not_valid_string_field(object fieldValue)
        {
            var value = validator.TryGetString("Teste", fieldValue, 1, 1);

            var isValid = validator.IsExtractionValid();

            Assert.Null(value);
            Assert.False(isValid);
        }


        [Fact]
        public void File_has_valid_decimal_field()
        {
            object fieldValue = (decimal)100.01;

            var value = validator.TryGetDecimal("Teste", fieldValue, 1, 1);

            var isValid = validator.IsExtractionValid();

            Assert.Equal(value, fieldValue);
            Assert.True(isValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData((object)"")]
        [InlineData((object)"a")]
        public void File_has_not_valid_decimal_field(object fieldValue)
        {
            var value = validator.TryGetDecimal("Teste", fieldValue, 1, 1);

            var isValid = validator.IsExtractionValid();

            Assert.False(isValid);
        }
    }
}
