namespace ExcelFileStorage.Api.Validators
{
    /// <summary>
    /// Валидация файла
    /// </summary>
    public static class FileValidator
    {
        /// <summary>
        /// Проверка расширения файла
        /// </summary>
        /// <param name="file">Файл</param>
        /// <param name="allowedExtensions">Допустимые расширения</param>
        /// <returns>Результат проверки</returns>
        public static bool IsFileExtensionAllowed(IFormFile file, string[] allowedExtensions)
        {
            var extension = Path.GetExtension(file.FileName);

            return allowedExtensions.Contains(extension);
        }
    }
}
