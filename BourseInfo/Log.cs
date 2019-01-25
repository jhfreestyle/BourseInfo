using System;
using System.IO;

namespace BourseInfo
{
    static class Log
    {
        private const string LogFilePath = @"log.txt";

        public static void Write(Exception ex, string url = null)
        {
            if (!string.IsNullOrEmpty(url))
            {
                Write(DateTime.Now + " Error loading " + url + ": " + ex.Message + Environment.NewLine);
            }
            else
            {
                Write(DateTime.Now + " Error: " + ex.Message + Environment.NewLine + "StackTrace: " + ex.StackTrace + Environment.NewLine + Environment.NewLine);
            }
        }

        public static void Write(string message)
        {
            using (StreamWriter writer = new StreamWriter(LogFilePath, true))
            {
                writer.WriteLine(message);
            }
        }
    }
}
