namespace ExcelFileStorage.Api.Exceptions
{
    /// <summary>
    /// Базовый класс обработки ошибок приложения
    /// </summary>
    public class ExcelFileStorageException : Exception
    {
        public ExcelFileStorageException(string? message) : base(message)
        {
        }

        public ExcelFileStorageException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
