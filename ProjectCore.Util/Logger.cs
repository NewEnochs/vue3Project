using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCore.Util
{
    public class Logger
    {
        private static readonly object _lock = new object();
        private static readonly string _logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        private const long MAX_FILE_SIZE = 50 * 1024 * 1024; // 50MB

        public static void Write(string message, string fileName= "日志信息") => WriteLog(fileName, message);
        public static void Info(string message) => WriteLog("INFO", message);
        public static void Error(string message) => WriteLog("ERROR", message);
        public static void Error(Exception ex) => WriteLog("ERROR", $"{ex.Message}\n{ex.StackTrace}");

        /// <summary>
        /// 获取当前可用的日志文件路径（自动处理文件大小限制）
        /// </summary>
        private static string GetLogFilePath(string fileName)
        {
            var date = System.DateTime.Now;
            var dateKey = date.ToString("yyyy-MM-dd");
            var logPath = Path.Combine(_logDirectory, dateKey);

            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);

            // 先检查基础文件
            string baseFilePath = Path.Combine(logPath, $"{fileName}.log");

            // 如果基础文件不存在或小于限制大小，直接返回
            if (!File.Exists(baseFilePath) || new FileInfo(baseFilePath).Length < MAX_FILE_SIZE)
                return baseFilePath;

            // 查找可用的序号文件
            int index = 1;
            while (true)
            {
                string indexedFilePath = Path.Combine(logPath, $"{fileName}_{index}.log");
                if (!File.Exists(indexedFilePath) || new FileInfo(indexedFilePath).Length < MAX_FILE_SIZE)
                {
                    return indexedFilePath;
                }
                index++;
            }
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="fileName">日志名称</param>
        /// <param name="message">写入信息</param>
        private static void WriteLog(string fileName, string message)
        {
            Task.Run(() =>
            {
                try
                {
                    var date = System.DateTime.Now;

                    // 获取当前可用的文件路径
                    string filePath = GetLogFilePath(fileName);

                    var logEntry = new StringBuilder();
                    logEntry.AppendLine($"┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                    logEntry.AppendLine($"┣ Time: {date:yyyy-MM-dd HH:mm:ss.fff}");
                    logEntry.AppendLine($"┣ Message: {message}");
                    logEntry.AppendLine($"┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\r\n");

                    lock (_lock)
                    {
                        // 双重检查：在写入前再次检查文件大小（防止并发写入导致超限）
                        if (File.Exists(filePath) && new FileInfo(filePath).Length >= MAX_FILE_SIZE)
                        {
                            // 重新获取新的文件路径
                            filePath = GetLogFilePath(fileName);
                        }

                        File.AppendAllText(filePath, logEntry.ToString(), Encoding.UTF8);
                    }
                }
                catch { /* 忽略日志写入错误 */ }
            });
        }

        /// <summary>
        /// 强制创建新日志文件（手动归档）
        /// </summary>
        public static void RollLog(string fileName = null)
        {
            Task.Run(() =>
            {
                try
                {
                    var date = System.DateTime.Now;
                    var dateKey = date.ToString("yyyy-MM-dd");
                    var logPath = Path.Combine(_logDirectory, dateKey);

                    if (!Directory.Exists(logPath))
                        return;

                    if (string.IsNullOrEmpty(fileName))
                    {
                        // 滚动所有日志文件
                        var files = Directory.GetFiles(logPath, "*.log");
                        foreach (var file in files)
                        {
                            var name = Path.GetFileNameWithoutExtension(file);
                            if (!name.Contains("_")) // 只处理基础文件
                            {
                                string newPath = Path.Combine(logPath, $"{name}_1.log");
                                File.Move(file, newPath);
                            }
                        }
                    }
                    else
                    {
                        string baseFilePath = Path.Combine(logPath, $"{fileName}.log");
                        if (File.Exists(baseFilePath))
                        {
                            // 找到当前最大的序号
                            int maxIndex = 0;
                            var existingFiles = Directory.GetFiles(logPath, $"{fileName}_*.log");
                            foreach (var file in existingFiles)
                            {
                                var name = Path.GetFileNameWithoutExtension(file);
                                var parts = name.Split('_');
                                if (parts.Length == 2 && int.TryParse(parts[1], out int index))
                                {
                                    if (index > maxIndex) maxIndex = index;
                                }
                            }

                            string newPath = Path.Combine(logPath, $"{fileName}_{maxIndex + 1}.log");
                            File.Move(baseFilePath, newPath);
                        }
                    }
                }
                catch { /* 忽略归档错误 */ }
            });
        }

        /// <summary>
        /// 清理旧日志文件（保留最近N天）
        /// </summary>
        public static void CleanOldLogs(int daysToKeep = 30)
        {
            Task.Run(() =>
            {
                try
                {
                    if (!Directory.Exists(_logDirectory))
                        return;

                    var cutoffDate = System.DateTime.Now.AddDays(-daysToKeep);
                    var directories = Directory.GetDirectories(_logDirectory);

                    foreach (var dir in directories)
                    {
                        if (System.DateTime.TryParseExact(Path.GetFileName(dir), "yyyy-MM-dd",
                            System.Globalization.CultureInfo.InvariantCulture,
                            System.Globalization.DateTimeStyles.None, out System.DateTime dirDate))
                        {
                            if (dirDate < cutoffDate)
                            {
                                Directory.Delete(dir, true);
                            }
                        }
                    }
                }
                catch { /* 忽略清理错误 */ }
            });
        }
    }
}