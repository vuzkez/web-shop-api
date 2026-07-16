namespace MyShop.WebApi.Applications.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entityName, object key)
            : base($"Cущность {entityName} с ключем {key} не найдена") { }
    }
}
