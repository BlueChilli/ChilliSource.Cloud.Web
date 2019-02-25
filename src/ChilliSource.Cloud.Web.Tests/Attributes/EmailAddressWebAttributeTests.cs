using ChilliSource.Cloud.Web.Attributes;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace ChilliSource.Cloud.Web.Tests
{
    public class EmailAddressWebAttributeTests
    {

        [Theory]
        [InlineData("joe@example.com", true)]
        [InlineData("jane+12345@example.com.au", true)]
        [InlineData("x@x.xx", true)]
        [InlineData("example.com", false)]
        [InlineData("@example.com", false)]
        [InlineData("joe@example.com.", false)]
        [InlineData("joe@example..com.au", false)]
        [InlineData("joe..13@example.com", false)]
        [InlineData("111111111111111111", false)]

        public void EmailAddressWebAttribute_ShouldBeValidatedCorrectly_WhenEmailAddressIsGiven(string emailaddress, bool shouldBeCorrect)
        {
            var emailAddressWebAttribute = new EmailAddressWebAttribute();
            if (shouldBeCorrect)
            {
                Assert.True(emailAddressWebAttribute.IsValid(emailaddress));
            }
            else
            {
                Assert.False(emailAddressWebAttribute.IsValid(emailaddress));
            }
        }

        [Theory]
        [InlineData("joe@example.com.")]
        [InlineData("joe@example..com.au")]
        public void EmailAddressWebAttribute_ShouldBeValidatedIncorrectly_WhenEdgeCaseEmailAddressIsGiven(string emailaddress)
        {
            var emailAddressAttribute = new EmailAddressAttribute();
            Assert.True(emailAddressAttribute.IsValid(emailaddress));
        }

    }
}
