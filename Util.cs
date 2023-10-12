using System.Configuration;

namespace Library
{
    public static class Util
    {
        private static readonly string ISBNregex =
            @"^(?:ISBN(?:-1[03])?:? )?(?=[0-9X]{10}$|(?=(?:[0-9]+[- ]){3})[- 0-9X]{13}$|97[89][0-9]{10}$|(?=(?:[0-9]+[- ]){4})[- 0-9]{17}$)(?:97[89][- ]?)?[0-9]{1,5}[- ]?[0-9]+[- ]?[0-9]+[- ]?[0-9X]$";

        public static bool IsStringMatchingPattern(string pattern, string str)
        {
            RegexStringValidator regexStringValidator = new RegexStringValidator(pattern);
            if (regexStringValidator.CanValidate(str.GetType()))
            {
                try
                {
                    regexStringValidator.Validate(str);
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        public static bool validateISBN(string ISBN)
        {
            return IsStringMatchingPattern(ISBNregex, ISBN);
        }

        public static bool displayMessage(bool condition, string messageToDisplay)
        {
            if (condition)
            {
                Console.WriteLine(messageToDisplay);
            }
            return condition;
        }
    }
}
