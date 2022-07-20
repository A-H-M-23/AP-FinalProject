using System.Text.RegularExpressions;

namespace Business.Validations
{
    public static class Email
    {
        /// <summary>
        /// This Method Is Check Email And Validate Email
        /// </summary>
        /// <param name="email">Email String Input</param>
        /// <returns>It Returns True For Valid Emails And False For Invalid Emails</returns>
        public static bool EmailCheck(string email)
        {
            string pattern = @"^[A-Za-z0-9._]{1,256}@[A-Za-z0-9]{1,256}[.][A-Za-z]{2,4}[.]{0,1}[A-Za-z]{0,4}";
            if (Regex.IsMatch(email, pattern))
                return true;
            else
                return false;
        }
    }
}
