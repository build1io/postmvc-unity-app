using System;
using System.Collections.Generic;
using System.IO;

namespace Build1.PostMVC.Unity.App.Modules.Logging
{
    public sealed class LogFile
    {
        public readonly string path;

        internal LogFile(string path)
        {
            this.path = path;
        }
        
        public string GetNameWithoutExtension()
        {
            return Path.GetFileNameWithoutExtension(path);
        }
        
        public string GetName()
        {
            return Path.GetFileName(path);
        }

        public string ReadAll()
        {
            return File.ReadAllText(path);
        }

        public string ReadLines(int lineLimit, bool addLimitMessage)
        {
            return ReadLines(lineLimit, addLimitMessage, out _);
        }
        
        public string ReadLines(int lineLimit, out bool exceedsLimit)
        {
            return ReadLines(lineLimit, false, out exceedsLimit);
        }

        public string ReadLines(int lineLimit, bool addLimitMessage, out bool exceedsLimit)
        {
            exceedsLimit = false;
            
            var queue = new Queue<string>(lineLimit);
            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (queue.Count == lineLimit)
                    {
                        queue.Dequeue();
                        exceedsLimit = true;
                    }
                        
                    queue.Enqueue(line);
                }
            }
            
            var log = string.Join(Environment.NewLine, queue);

            if (exceedsLimit && addLimitMessage)
                log = $"========== TRIMMED TO LIMIT ==========\n\n{log}";

            return log;
        }
    }
}