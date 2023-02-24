using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Unity.App.Utils.Path;
using UnityEngine;

namespace Build1.PostMVC.Unity.App.Modules.Logging.Impl
{
    internal sealed class LogController : ILogController
    {
        [Inject] public IEventDispatcher Dispatcher { get; set; }

        private readonly StringBuilder _records = new();
        private          int           _recordsCount;
        private readonly DateTime      _recordsDate = DateTime.UtcNow;

        [PostConstruct]
        public void PostConstruct()
        {
            Application.logMessageReceivedThreaded += OnLogReceived;
        }

        [PreDestroy]
        public void PreDestroy()
        {
            Application.logMessageReceivedThreaded -= OnLogReceived;
        }

        /*
         * Public.
         */

        public void RecordMessage(string message, LogLevel level, bool forceFlush)
        {
            if (level < Logging.RecordLevel)
                return;

            lock (_records)
            {
                _records.AppendLine(message);
                _recordsCount++;

                if (forceFlush || (Logging.FlushThreshold > 0 && _recordsCount >= Logging.FlushThreshold))
                    FlushLog();
            }
        }

        public string GetLog()
        {
            return _recordsCount > 0 ? _records.ToString() : string.Empty;
        }

        public void FlushLog()
        {
            if (_recordsCount < 1)
                return;

            var log = _records.ToString();

            _records.Clear();
            _recordsCount = 0;

            if (Logging.SaveToFile)
            {
                WriteToFile(log, out var newFileCreated);

                if (newFileCreated)
                    DeleteOldFiles();
            }
            else if (Logging.RecordsHistory == 0)
            {
                DeleteOldFiles();
            }

            Dispatcher.Dispatch(LogEvent.Flush, log);
        }

        public List<LogFile> GetLogFiles()
        {
            var folderPath = Path.Combine(PathUtil.GetPersistentDataPath(), "Logs");
            if (!Directory.Exists(folderPath))
                return null;

            var paths = Directory.EnumerateFiles(folderPath, "*.log", SearchOption.TopDirectoryOnly).ToArray();
            var infos = new List<LogFile>(paths.Length);

            foreach (var path in paths)
                infos.Add(new LogFile(path));

            return infos;
        }

        public LogFile GetLastLogFile()
        {
            var folderPath = Path.Combine(PathUtil.GetPersistentDataPath(), "Logs");
            if (!Directory.Exists(folderPath))
                return null;

            var directory = new DirectoryInfo(folderPath);
            var file = directory.GetFiles()
                                .OrderByDescending(f => f.LastWriteTime)
                                .First();

            return new LogFile(Path.Combine(folderPath, file.FullName));
        }

        public void DeleteLogFiles()
        {
            var folderPath = Path.Combine(PathUtil.GetPersistentDataPath(), "Logs");
            if (!Directory.Exists(folderPath))
                return;

            var directory = new DirectoryInfo(folderPath);

            foreach (var file in directory.GetFiles())
                file.Delete();

            foreach (var dir in directory.GetDirectories())
                dir.Delete(true);
        }

        /*
         * Private.
         */

        private void WriteToFile(string logs, out bool newFileCreated)
        {
            newFileCreated = false;

            try
            {
                var folderPath = Path.Combine(PathUtil.GetPersistentDataPath(), "Logs");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var fileName = $"{_recordsDate:MM.dd.yyyy HH.mm.ss}.log";
                var filePath = Path.Combine(folderPath, fileName);

                if (File.Exists(filePath))
                {
                    File.AppendAllText(filePath, logs);
                }
                else
                {
                    File.WriteAllText(filePath, logs);
                    newFileCreated = true;
                }
            }
            catch
            {
                // Ignore.
            }
        }

        private void DeleteOldFiles()
        {
            var folderPath = Path.Combine(PathUtil.GetPersistentDataPath(), "Logs");
            if (!Directory.Exists(folderPath))
                return;

            var directory = new DirectoryInfo(folderPath);
            var files = directory.GetFiles()
                                 .OrderByDescending(f => f.LastWriteTime)
                                 .ToArray();

            if (files.Length <= Logging.RecordsHistory)
                return;

            for (var i = Logging.RecordsHistory; i < files.Length; i++)
                files.ElementAt(i).Delete();
        }

        /*
         * Event Handlers.
         */

        private void OnLogReceived(string logString, string stackTrace, LogType type)
        {
            if (!Logging.Record || Logging.RecordLevel == LogLevel.None)
                return;

            var level = type switch
            {
                LogType.Error     => LogLevel.Error,
                LogType.Assert    => LogLevel.Error,
                LogType.Warning   => LogLevel.Warning,
                LogType.Log       => LogLevel.Debug,
                LogType.Exception => LogLevel.Error,
                _                 => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

            var isError = level == LogLevel.Error;
            var message = isError ? $"{logString}\n{stackTrace}\n" : logString;
            RecordMessage(message, level, isError);
        }
    }
}