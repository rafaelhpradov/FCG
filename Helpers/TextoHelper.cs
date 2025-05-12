using System.Text.RegularExpressions;

namespace FCG.Helpers
{
    public class TextoHelper
    {
        public bool EmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string padrao = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            return Regex.IsMatch(email, padrao);
        }

        public bool SenhaValida(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha))
                return false;

            var hasMinimumLength = senha.Length >= 6;
            var hasUpperChar = new Regex(@"[A-Z]+").IsMatch(senha);
            var hasLowerChar = new Regex(@"[a-z]+").IsMatch(senha);
            var hasNumber = new Regex(@"[0-9]+").IsMatch(senha);
            var hasSpecialChar = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]").IsMatch(senha);

            return hasMinimumLength && hasUpperChar && hasLowerChar && hasNumber && hasSpecialChar;
        }
    }
}
