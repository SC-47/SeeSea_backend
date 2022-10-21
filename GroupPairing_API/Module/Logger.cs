//-----------------------------------------------------------------------
// <copyright file="Logger.cs" company="Cmoney">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace GroupPairing_API.Module
{
    using GroupPairing_API.Interface;
    using System.IO;
    using System.Text;

    public class Logger : ILogger
    {
        /// <summary>
        /// 放Log的根目錄
        /// </summary>
        private readonly string RootName;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="dirPath">log檔儲存的根目錄</param>
        public Logger(string dirPath)
        {
            RootName = dirPath;
            if (!Directory.Exists(RootName))
            {
                Directory.CreateDirectory(RootName);
            }
        }

        /// <summary>
        /// Write log to the local file.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="fileName">The name of the writing file.</param>
        public void AddLog(string content, string fileName)
        {
            string path = $"{RootName}\\{fileName}.log";

            using var fileStream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            using var streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
            streamWriter.WriteLine(content);
        }
    }
}