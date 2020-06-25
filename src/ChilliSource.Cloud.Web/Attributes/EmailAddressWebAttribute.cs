using ChilliSource.Cloud.Core.Email;
using System;
using System.ComponentModel.DataAnnotations;

namespace ChilliSource.Cloud.Web
{
    /// <summary>
    /// Validates email address properties. 
    /// </summary>
    public class EmailAddressWebAttribute : DataTypeAttribute
    {

        public EmailAddressWebAttribute()
            : base(DataType.EmailAddress)
        {
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;
            if (base.IsValid(value))
            {
                var emailAddressAttribute = new EmailAddressAttribute();
                if (emailAddressAttribute.IsValid(value))
                {
                    var s = value as string;
                    if (String.IsNullOrEmpty(s)) return false;
                    return s.IsValidEmailAddress();
                }
            }

            return false;
        }
    }
}
