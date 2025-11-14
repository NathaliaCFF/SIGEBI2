namespace UI2.Services
{
    public class ValidationService
    {
        public bool ValidateRequired(string value, string fieldName, out string message)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                message = $"El campo {fieldName} es obligatorio.";
                return false;
            }

            message = string.Empty;
            return true;
        }

        public bool ValidateEmail(string value, out string message)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                message = "El correo electrónico es obligatorio.";
                return false;
            }

            if (!value.Contains("@"))
            {
                message = "El correo electrónico no tiene un formato válido.";
                return false;
            }

            message = string.Empty;
            return true;
        }
    }
}