using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Serenity_Craft.Models.MyValidations
{
    public class EmailValidation : ValidationAttribute
    {
        private bool BeUnique(string email, int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return db.Contacts.FirstOrDefault(c => c.Email == email && c.PublisherId != id) == null;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var contact = (Contact)validationContext.ObjectInstance;

            string email = contact.Email;
            int id = contact.PublisherId;

            if (email == null)
            {
                return new ValidationResult("Email field is required!");
            }

            Regex regex = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
            if (!regex.IsMatch(email))
            {
                return new ValidationResult("Invalid email!");
            }

            if (!BeUnique(email, id))
            {
                return new ValidationResult("Already exists in our database!");
            }

            return ValidationResult.Success;
        }
    }
}