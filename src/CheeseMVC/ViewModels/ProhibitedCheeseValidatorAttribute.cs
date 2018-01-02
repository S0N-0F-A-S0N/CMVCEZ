using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CheeseMVC.ViewModels
{
    /// <summary>
    /// Simple custom validator example
    /// </summary>
    public class ProhibitedCheeseValidatorAttribute : ValidationAttribute
    {
        private readonly string[] prohibitedNames;

        public ProhibitedCheeseValidatorAttribute(string[] prohibitedNames)
            : base("This cheese has been blacklisted!")
        {
            this.prohibitedNames = prohibitedNames;
        }

        public override bool IsValid(object value)
        {
            string cheeseName = (string) value;
            return !prohibitedNames.Contains(cheeseName);
        }
    }
}