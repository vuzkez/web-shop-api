namespace MyShop.WebApi.Applications.Common.Exceptions
{
    public class CustomValidationException : Exception
    {
        public Dictionary<string, string>? Errors { get; private set; } = null;
        public CustomValidationException(Dictionary<string, string> errors) 
            : base("Произошла ошибка валидации") { Errors = errors; }

        public CustomValidationException(string message)
            : base(message) { }
    }
}
