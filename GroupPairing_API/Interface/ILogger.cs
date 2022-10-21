//-----------------------------------------------------------------------
// <copyright file="ILogger.cs" company="Cmoney">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace GroupPairing_API.Interface
{
    /// <summary>
    /// The module for writing log.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Write log to the local file.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="fileName">The name of the writing file.</param>
        public void AddLog(string content, string fileName);
    }
}