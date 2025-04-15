using ProductApi.DTOs;
using System.ComponentModel.DataAnnotations;

namespace ProductApi.Tests.DTOs
{
    public class DtoValidationTests
    {
        private List<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }

        [Fact]
        public void BaseProductDto_ShouldFailValidation_WhenNameIsMissing()
        {
            var dto = new BaseProductDto
            {
                Name = "", // invalid
                Description = "Test",
                StockAvailable = 10,
                Price = 100
            };

            var results = ValidateModel(dto);

            Assert.Contains(results, r => r.MemberNames.Contains("Name"));
        }

        [Fact]
        public void BaseProductDto_ShouldFailValidation_WhenPriceIsZero()
        {
            var dto = new BaseProductDto
            {
                Name = "Product",
                Description = "Test",
                StockAvailable = 10,
                Price = 0 // invalid
            };

            var results = ValidateModel(dto);

            Assert.Contains(results, r => r.MemberNames.Contains("Price"));
        }

        [Fact]
        public void BaseProductDto_ShouldPassValidation_WithValidData()
        {
            var dto = new BaseProductDto
            {
                Name = "Valid Product",
                Description = "Looks good",
                StockAvailable = 5,
                Price = 9.99m
            };

            var results = ValidateModel(dto);

            Assert.Empty(results);
        }
    }
}
