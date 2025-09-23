using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace starterTemplate
{
    public interface ILogger
    {
        void Initialize();
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message, Exception? ex = null);
    }

    public class FileLogger : ILogger
    {
        private readonly IConfiguration _configuration;
        private readonly string _loggerName;
        private string _logFilePath = string.Empty;
        private readonly object _lock = new object();
        private static readonly Dictionary<string, string> _sharedTimestamp = new Dictionary<string, string>();
        private static readonly object _timestampLock = new object();

        public FileLogger(IConfiguration configuration, string loggerName = "main")
        {
            _configuration = configuration;
            _loggerName = loggerName;
        }

        public void Initialize()
        {
            try
            {
                string logDirectory = _configuration["Logging:Directory"] ?? "logs";
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

                // Create date-based directory structure: logs/yyyy/MM/dd
                DateTime now = DateTime.Now;
                string datePath = Path.Combine(logDirectory, now.Year.ToString(), now.Month.ToString("00"), now.Day.ToString("00"));
                string fullLogPath = Path.Combine(baseDirectory, datePath);
                Directory.CreateDirectory(fullLogPath);

                // Get or create shared timestamp for all loggers created at app startup
                string timestamp;
                lock (_timestampLock)
                {
                    if (!_sharedTimestamp.ContainsKey("startup"))
                    {
                        _sharedTimestamp["startup"] = now.ToString("HHmmss");
                    }
                    timestamp = _sharedTimestamp["startup"];
                }

                // Create log file with HHMMSS_name format
                _logFilePath = Path.Combine(fullLogPath, $"{timestamp}_{_loggerName}.log");

                LogInfo($"Logger '{_loggerName}' initialized.");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{_loggerName.ToUpper()}_INIT_FAIL] Failed to initialize logger: {ex.Message}");
                Console.ResetColor();
                _logFilePath = string.Empty;
            }
        }

        /// <summary>
        /// Logs an informational message to console (Green) and to log file.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void LogInfo(string message)
        {
            Log("INFO", message);
        }

        /// <summary>
        /// Logs a warning message to console (Yellow) and to log file.
        /// </summary>
        /// <param name="message">The warning message to log.</param>
        public void LogWarning(string message)
        {
            Log("WARN", message);
        }

        /// <summary>
        /// Logs an error message to console (Red) and to log file, optionally including details from an exception.
        /// </summary>
        /// <param name="message">The error message to log.</param>
        /// <param name="ex">The exception associated with the error, if any. Its type, message, and stack trace will be logged.</param>
        public void LogError(string message, Exception? ex = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(message);
            if (ex != null)
            {
                sb.AppendLine($" Exception: {ex.GetType().Name} - {ex.Message}");
                sb.AppendLine($" StackTrace: {ex.StackTrace}");
            }
            Log("ERROR", sb.ToString().TrimEnd());
        }

        /// <summary>
        /// Private helper method to write a log entry to the console (with color) and to the log file.
        /// Prepends a timestamp and log level to the message.
        /// Console output is colored based on level and includes logger name. File output is plain text.
        /// This method is thread-safe for file writing.
        /// </summary>
        /// <param name="level">The log level (e.g., "INFO", "WARN", "ERROR").</param>
        /// <param name="message">The message content to log.</param>
        private void Log(string level, string message)
        {
            string logEntryContent = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{level}] {message}";
            string consoleEntryContent = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{_loggerName.ToUpper()}:{level}] {message}";

            ConsoleColor originalConsoleColor = Console.ForegroundColor;
            switch (level.ToUpperInvariant())
            {
                case "INFO":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case "WARN":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case "ERROR":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
            }
            Console.WriteLine(consoleEntryContent);
            Console.ForegroundColor = originalConsoleColor;

            if (!string.IsNullOrEmpty(_logFilePath))
            {
                lock (_lock)
                {
                    try
                    {
                        File.AppendAllText(_logFilePath, logEntryContent + Environment.NewLine);
                    }
                    catch (Exception ex)
                    {
                        // Fallback error message to console if file write fails
                        ConsoleColor originalColorFallback = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{_loggerName.ToUpper()}:LOG_WRITE_FAIL] Failed to write to log file '{_logFilePath}': {ex.Message}. Original log: [{level}] {message}");
                        Console.ForegroundColor = originalColorFallback;
                    }
                }
            }
            // If _logFilePath is empty (e.g., Initialize failed), messages will only go to the console.
        }
    }

    /// <summary>
    /// Factory class to create named logger instances.
    /// </summary>
    public static class LoggerFactory
    {
        /// <summary>
        /// Creates a new logger instance with the specified name.
        /// </summary>
        /// <param name="configuration">The configuration instance.</param>
        /// <param name="name">The name for this logger instance (e.g., "main", "api", "db").</param>
        /// <returns>A new FileLogger instance.</returns>
        public static ILogger CreateLogger(IConfiguration configuration, string name)
        {
            return new FileLogger(configuration, name);
        }
    }
}