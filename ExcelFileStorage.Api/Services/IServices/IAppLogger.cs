namespace ExcelFileStorage.Api.Services.IServices
{
    /// <summary>
    /// Объект логирования 
    /// </summary>
    public interface IAppLogger
    {
        /// <summary>
        /// Добавление записи в лог
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="parameters">Параметры</param>
        /// <returns></returns>
        void Log(string message, Dictionary<string, object> parameters = null);
    }
}
