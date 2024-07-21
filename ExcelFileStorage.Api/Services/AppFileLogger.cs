using ExcelFileStorage.Api.Services.IServices;
using System.IO;
using System.Text.Json;

namespace ExcelFileStorage.Api.Services
{
    /// <summary>
    /// Логер в файл
    /// </summary>
    public class AppFileLogger : IAppLogger
    {
        private static object _lock = new object();
        private readonly string _filePath;

        private const string _logFileName = "AppLog.txt";

        public AppFileLogger()
        {
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), Constants.LogsDirecoryName);

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            _filePath= Path.Combine(directoryPath, _logFileName);
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
                File.AppendAllText(_filePath, logMsgDetailsJson + Environment.NewLine);
            }
        }
    }
}
