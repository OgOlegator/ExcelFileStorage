using ExcelFileStorage.Api.Services.IServices;
using System.Text.Json;

namespace ExcelFileStorage.Api.Services
{
    /// <summary>
    /// Логер в файл
    /// </summary>
    public class AppFileLogger : IAppLogger
    {
        private object _lock = new object();

        private const string _logFileName = "AppLog.txt";

        private readonly IFileOnServer _fileOnServer;

        public AppFileLogger(IFileOnServer fileOnServer)
        {
            _fileOnServer = fileOnServer;
        }

        public void Log(string message, Dictionary<string, object> parameters = null)
        {
            var logMsgDetailsJson = JsonSerializer.Serialize(new
            {
                message,
                parameters
            });

            lock (_lock)
            {
                _fileOnServer.CreateOrWriteToEnd(_logFileName, Constants.LogsDirecoryName, logMsgDetailsJson);
            }
        }
    }
}
