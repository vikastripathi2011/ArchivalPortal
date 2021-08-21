using System;
using System.Configuration;
using System.IO;

namespace AWSS3ServiceLayer.ServiceClass
{
    public class Log
    {
        static string filePath = string.Empty, logFilePath = string.Empty;
        Log()
        {

        }
        public static void Failed(string exceptionText, string logCreationDateTime, string logPath)
        {
            logFilePath = string.Empty;
            logFilePath = Path.Combine(logPath, GlobalVariable.logCreationDateTime);

            if (!Directory.Exists(logFilePath))
            {
                Directory.CreateDirectory(logFilePath);
            }

            filePath = Path.Combine(logFilePath, string.Concat(("Failed " + GlobalVariable.logCreationDateTime + ".txt")));

            using (FileStream fileStream = new FileStream(filePath, FileMode.Append))

            using (StreamWriter streamWriter = new StreamWriter(fileStream))
            {
                streamWriter.WriteLine("****************************" + DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm:ss") + "*******************************");
                streamWriter.WriteLine("Exception : " + exceptionText);
                streamWriter.WriteLine("");
            }
        }
        public static void Success(string statusText, string logCreationDateTime, string logPath)
        {
            logFilePath = string.Empty;
            logFilePath = Path.Combine(logPath, GlobalVariable.logCreationDateTime);

            if (!Directory.Exists(logFilePath))
            {
                Directory.CreateDirectory(logFilePath);
            }
            filePath = Path.Combine(logFilePath, string.Concat(("Success " + GlobalVariable.logCreationDateTime + ".txt")));

            using (FileStream fileStream = new FileStream(filePath, FileMode.Append))

            using (StreamWriter streamWriter = new StreamWriter(fileStream))
            {
                streamWriter.WriteLine("****************************" + DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm:ss") + "*******************************");
                streamWriter.WriteLine("Status          : " + statusText);
                streamWriter.WriteLine("");
            }
        }

    }

    public static class GlobalVariable
    {
        public static string logCreationDateTime = DateTime.Now.ToString("dd-MMMM-yyyy HH-mm-ss");
    }
}