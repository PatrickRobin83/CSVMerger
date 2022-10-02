/*
 * -----------------------------------------------------------------------------
 *	 
 *   Filename		:   CsvFileMergerService.cs
 *   Date			:   2022-09-30
 *   All rights reserved
 * 
 * -----------------------------------------------------------------------------
 * @author     Patrick Robin <p.robin@smartperform.de>
 * @Version      1.0.0
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using CSVMerger.Core.Enums;
using CSVMerger.Core.Models;

namespace CSVMerger.Core.Services
{
    /// <summary>
    /// This Class reads content from .csv files and merge them together in a new .csv file
    /// </summary>
    public static class CsvFileMergerService
    {

        #region Fields

        private static string _outputFilename;
        private static string _outputfileExtension = ".csv";
        private static string _outputPath;
        private static ObservableCollection<StatisticFile> _statisticFiles;
        private static List<string> _fileContent;
        private static StreamWriter sw;
        private static int _fileExistNameAppendix;


        #endregion

        #region Properties

        #endregion

        #region Constructor

        #endregion

        #region Methods
        /// <summary>
        /// Creates a new .csv file and merges the content from the given files
        /// </summary>
        /// <param name="outputFileName">Name of the output file</param>
        /// <param name="outputpath">Path for the output file</param>
        /// <param name="statisticFiles">Collection of .csv files to merge</param>
        public static void MergeAndCreateFile(string outputFileName, string outputpath,
            ObservableCollection<StatisticFile> statisticFiles)
        {
            if (outputFileName.EndsWith(".csv"))
            {
                _outputFilename = outputFileName;
            }
            else
            {
                _outputFilename = $"{outputFileName}.csv";
            }

            _outputPath = outputpath;
            _statisticFiles = statisticFiles;

            try
            {
                while (File.Exists($@"{_outputPath}\{_outputFilename}"))
                {
                    _fileExistNameAppendix++;
                    _outputFilename = $"{outputFileName}_{_fileExistNameAppendix}{_outputfileExtension}";
                }
                sw = new StreamWriter($@"{_outputPath}\{_outputFilename}", true);


                for (int i = 0; i < statisticFiles.Count; i++)
                {
                    _fileContent = new List<string>();
                    if (i == 0)
                    {
                        _fileContent.AddRange(File.ReadAllLines(statisticFiles[i].Path));

                    }
                    else
                    {
                        _fileContent.AddRange(File.ReadAllLines(statisticFiles[i].Path));
                        _fileContent.RemoveAt(0);
                    }

                    foreach (string line in _fileContent)
                    {
                        sw.WriteLine(line);
                    }
                }
                sw.Close();
                sw.Dispose();
            }
            catch (Exception ex)
            {
                Logger.Log(LogState.Error, ex.Message);
            }
        }
        #endregion

    }
}