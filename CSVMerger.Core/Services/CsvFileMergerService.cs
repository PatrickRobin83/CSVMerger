/*
 * -----------------------------------------------------------------------------
 *	 
 *   Filename		:   CsvFileMergerService.cs
 *   Date			:   2022-09-30 11:11:32
 *   All rights reserved
 * 
 * -----------------------------------------------------------------------------
 * @author     Patrick Robin <p.robin@smartperform.de>
 * @Version      1.0.0
 */

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using CSVMerger.Core.Models;

namespace CSVMerger.Core.Services
{
    public static class CsvFileMergerService
    {

        #region Fields

        private static string _outputfilename;
        private static string _outputPath;
        private static StatisticFile _mergedFile;
        private static ObservableCollection<StatisticFile> _statisticFiles;
        private static List<string> _fileContent;
        private static StreamWriter sw;


        #endregion

        #region Properties

        #endregion

        #region Constructor

        #endregion

        #region Methods

        public static void MergeAndCreateFile(string outputFileName, string outputpath,
            ObservableCollection<StatisticFile> statisticFiles)
        {
            if (outputFileName.EndsWith(".csv"))
            {
                _outputfilename = outputFileName;
            }
            else
            {
                _outputfilename = outputFileName + ".csv";
            }

            _outputPath = outputpath;
            _statisticFiles = statisticFiles;

            if (File.Exists(_outputPath + "\\" + outputFileName))
            {
                File.Delete(_outputPath + "\\" + outputFileName);
            }
            sw = new StreamWriter(_outputPath + "\\" + outputFileName, true);


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
        #endregion

    }
}