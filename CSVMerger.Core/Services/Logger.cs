/*
*----------------------------------------------------------------------------------
*          Filename:	Logger.cs
*          Date:        2022.10.02
*          All rights reserved
*
*----------------------------------------------------------------------------------
* @author Patrick Robin <p.robin@smartperform.de>
*/


using CSVMerger.Core.Enums;
using System;
using System.IO;

namespace CSVMerger.Core.Services
{
    public class Logger
    {
        #region Fields

        private static readonly string _logFilePath = $@"{Environment.CurrentDirectory}\Logs\";
        private static readonly string _dateFormatString = "yyyy_MM_dd";
        private static readonly string _dateFormatStringForLogFileText = "dd.MM.yyyy-HH:mm:ss";

        #endregion

        #region Properties

        #endregion

        #region Constructor

        #endregion

        #region Methods
        /// <summary>
        /// Creates the LogFile and writes the Date | State | Message into the file
        /// </summary>
        /// <param name="state">LogState Info, Warning or Error</param>
        /// <param name="message">The Logmessage as String</param>
        public static void Log(LogState state, string message)
        {
            if (!Directory.Exists(_logFilePath))
            {
                Directory.CreateDirectory(_logFilePath);
            }
            if (File.Exists(@$"{_logFilePath}\{DateTime.Now.ToString(_dateFormatString)}_log.txt"))
            {
                File.AppendAllText(_logFilePath + $@"\{DateTime.Now.ToString(_dateFormatString)}_log.txt", $"{DateTime.Now.ToString(_dateFormatStringForLogFileText)}||{state.ToString()}||{message}\r\n");

            }
            else
            {
                File.WriteAllText($@"{_logFilePath}\{DateTime.Now.ToString(_dateFormatString)}_log.txt", $"{DateTime.Now.ToString(_dateFormatStringForLogFileText)}||{state.ToString()}||{message}\r\n");
            }
        }
        #endregion

        #region Commands

        #endregion
    }
}