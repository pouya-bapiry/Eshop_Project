

namespace Eshop.Application.Utilities
{
    public static class Logger
    {
        private static readonly string _logFolderPath = Path.Combine(AppContext.BaseDirectory, "Logs");

        private static string GetLogFilePath()
        {
            // Generate a fresh path each time based on the current date
            var fileName = $"log-{DateTime.Now.ToStringShamsiDate()}.txt";
            return Path.Combine(_logFolderPath, fileName);
        }

        public static void ShowError(Exception ex)
        {
            try
            {
                if (!Directory.Exists(_logFolderPath))
                    Directory.CreateDirectory(_logFolderPath);

                var filePath = GetLogFilePath();

                var errorMessage = $"Error ===> Time : {DateTime.Now.ToStringShamsiDate()}" +
                                   $" ***** , Message ===> {ex.Message} \n Exception ===> {ex.ToString()}\n ***************************************************";

                File.AppendAllLines(filePath, new List<string> { errorMessage });
            }
            catch
            {
                // Avoid throwing inside logger
            }
        }
    }
}

