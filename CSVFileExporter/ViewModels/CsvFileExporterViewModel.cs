/*
 * -----------------------------------------------------------------------------
 *	 
 *   Filename		:   CsvFileExporterViewModel.cs
 *   Date			:   2022-09-29
 *   All rights reserved
 * 
 * -----------------------------------------------------------------------------
 * @author     Patrick Robin <p.robin@smartperform.de>
 * @Version      1.0.0
 */

using CSVMerger.Core.Events;
using CSVMerger.Core.Models;
using CSVMerger.Core.Services;
using FolderBrowserEx;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using CSVMerger.Core.Enums;
using FolderBrowserDialog = FolderBrowserEx.FolderBrowserDialog;

namespace CSVFileExporter.ViewModels
{
    public class CsvFileExporterViewModel : BindableBase
    {
        #region Fields

        private string _headline;
        private IEventAggregator _eventAggregator;
        private ObservableCollection<StatisticFile> _statisticFilesToMerge = new ObservableCollection<StatisticFile>();
        private bool _canMergAndExport = false;
        private string _outputFolderPath;
        private IFolderBrowserDialog _folderBrowserDialog;
        private string _exportFileName = "";

        #endregion

        #region Properties
        /// <summary>
        /// Headline from the export section
        /// </summary>
        public string Headline
        {
            get { return _headline; }
            set { SetProperty(ref _headline, value); }
        }
        /// <summary>
        /// Collection of files to merge
        /// </summary>
        public ObservableCollection<StatisticFile> StatisticFilesToMerge
        {
            get { return _statisticFilesToMerge; }
            set
            {
                SetProperty(ref _statisticFilesToMerge, value);
                MergeAndExportFileCommand.RaiseCanExecuteChanged();

            }
        }
        /// <summary>
        /// Bool to determine the state of the button IsEnabled = true/false 
        /// </summary>
        public bool CanMergAndExport
        {
            get { return _canMergAndExport; }
            set
            {
                SetProperty(ref _canMergAndExport, value);
            }
        }
        /// <summary>
        /// Command to Open a folder selection dialog
        /// </summary>
        public DelegateCommand OpenOutputFolderDialogCommand { get; set; }
        /// <summary>
        /// Command to Execute the Merge an export function
        /// </summary>
        public DelegateCommand MergeAndExportFileCommand { get; set; }
        /// <summary>
        /// Output path where the file will be exported to
        /// </summary>
        public string OutputFolderPath
        {
            get { return _outputFolderPath; }
            set
            {
                SetProperty( ref _outputFolderPath, value); 
                MergeAndExportFileCommand.RaiseCanExecuteChanged();
            }
        }
        /// <summary>
        /// Name of the export file 
        /// </summary>
        public string ExportFileName
        {
            get { return _exportFileName; }
            set
            {
                SetProperty(ref _exportFileName, value);
                MergeAndExportFileCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Constructor

        public CsvFileExporterViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<FromMergerToExporterEvent>().Subscribe(CloneCollectionToProperty);
            InitializeCommands();
            Headline = "Export .csv Datei";
        }

        #endregion

        #region Methods

        private void CloneCollectionToProperty(ObservableCollection<StatisticFile> filesToMerge)
        {
            StatisticFilesToMerge = filesToMerge;
        }
        /// <summary>
        /// Initializes all commands
        /// </summary>
        private void InitializeCommands()
        {
            OpenOutputFolderDialogCommand = new DelegateCommand(SelectOutputFolder);
            MergeAndExportFileCommand = new DelegateCommand(MergeAndExportCsvFile, CanMergeAndExportCsvFile);
        }
        /// <summary>
        /// Opens the FolderSelectDialog and sets the output path. 
        /// </summary>
        private void SelectOutputFolder()
        {
            _folderBrowserDialog = new FolderBrowserDialog();
            _folderBrowserDialog.Title = "Ausgabeverzeichnis";
            _folderBrowserDialog.InitialFolder = @"C:\";
            _folderBrowserDialog.AllowMultiSelect = false;

            if (_folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                OutputFolderPath = _folderBrowserDialog.SelectedFolder;
            }
        }
        /// <summary>
        /// Checks whether the button MergeAndExport can be pressed or whether it must be deactivated.
        /// </summary>
        /// <returns>bool</returns>
        private bool CanMergeAndExportCsvFile()
        {
            if (!string.IsNullOrEmpty(OutputFolderPath) && !string.IsNullOrEmpty(ExportFileName) && StatisticFilesToMerge.Count > 1)
            {
                CanMergAndExport = true;
            }
            else
            {
                CanMergAndExport = false;
            }

            return CanMergAndExport;
        }
        /// <summary>
        /// Calls the MergerService and shows a MessageBox after the MergerService finish
        /// </summary>
        private void MergeAndExportCsvFile()
        {
            CsvFileMergerService.MergeAndCreateFile(ExportFileName, OutputFolderPath,StatisticFilesToMerge);
            MessageBox.Show($"Dateien wurden zusammengeführt und liegen unter: {OutputFolderPath}\\{ExportFileName}.csv",
                "Erfolgreicher Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Logger.Log(LogState.Info, "Success Files Merged and Exported");
            _eventAggregator.GetEvent<ClearCollectionsEvent>().Publish();
            ClearCollections.ClearCollection(StatisticFilesToMerge);
            OutputFolderPath = "";
            ExportFileName = "";
            MergeAndExportFileCommand.RaiseCanExecuteChanged();
            Logger.Log(LogState.Info, "Application reset");
        }

        #endregion
    }
}
