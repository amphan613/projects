using System.Text.RegularExpressions;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

namespace DataSanitization.validator
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class XssValidatorAttribute : ValidationAttribute
    {

        public override bool IsValid(object obj)
        {
            bool result = true;
            
            if (obj != null && obj is string)
            {
                var input = obj.ToString();
                result = DetectXSS(input);
            }

            return result;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture,
              ErrorMessageString, name);
        }

        internal bool DetectXSS(string? input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            Regex rx = new Regex(@"^[a-zA-Z0-9]+$");

            return rx.IsMatch(input);

        }
    }
}
