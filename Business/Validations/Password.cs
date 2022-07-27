namespace Business.Validations
{
    public static class Password
    {
        /// <summary>
        /// This Method Is Check The Strength of Password
        /// Safe Password Has At Least A Upper A Lower A Number A Symbol and 8 Charachters Length
        /// </summary>
        /// <param name="password">The Password String Input</param>
        /// <returns>It Returns true For Safe Password and false For Unsafe Password</returns>
        public static bool PasswordSecurity(string password)
        {
            if (password.Length >= 8)
            {
                if (password.Any(char.IsNumber))
                {
                    if (password.Any(char.IsLower))
                    {
                        if (password.Any(char.IsUpper))
                        {
                            if (password.Any(char.IsSymbol) || password.Contains('!') || password.Contains('&') || password.Contains('?') || password.Contains('@') || password.Contains(';'))
                                return true;
                            else return false;
                        }
                        else return false;
                    }
                    else return false;
                }
                else return false;
            }
            else
                return false;
        }
    }
}
