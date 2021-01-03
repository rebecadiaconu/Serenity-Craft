using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Serenity_Craft.Models.MyValidations
{
    public class PhoneNumberValidation : ValidationAttribute
    {
        private bool BeUnique(string phoneNumber, int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return db.Contacts.FirstOrDefault(c => c.PhoneNumber == phoneNumber && c.PublisherId != id) == null;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Contact contact = (Contact)validationContext.ObjectInstance;

            string phoneNumber = contact.PhoneNumber;
            int id = contact.PublisherId;

            if (phoneNumber == null)
            {
                return new ValidationResult("Phone number field is required!");
            }

            Regex regex = new Regex(@"^0(\d{9})$");
            if (!regex.IsMatch(phoneNumber))
            {
                return new ValidationResult("Phone number must contain only digits, and the first one must be 0!");
            }

            if (phoneNumber.Length != 10)
            {
                return new ValidationResult("Phone number must contain 10 digits!!");
            }

            if (!BeUnique(phoneNumber, id))
            {
                return new ValidationResult("Already exists in our database!");
            }

            return ValidationResult.Success;
        }
    }
}