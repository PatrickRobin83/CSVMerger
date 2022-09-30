using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Forms;
using CSVMerger.Core.Events;
using CSVMerger.Core.Models;
using CSVMerger.Core.Services;
using FolderBrowserEx;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
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

        public string Headline
        {
            get { return _headline; }
            set { SetProperty(ref _headline, value); }
        }

        public ObservableCollection<StatisticFile> StatisticFilesToMerge
        {
            get { return _statisticFilesToMerge; }
            set
            {
                SetProperty(ref _statisticFilesToMerge, value);
                MergeAndExportFileCommand.RaiseCanExecuteChanged();

            }
        }

        public bool CanMergAndExport
        {
            get { return _canMergAndExport; }
            set
            {
                SetProperty(ref _canMergAndExport, value);
            }
        }

        public DelegateCommand OpenOutputFolderDialogCommand { get; set; }
        public DelegateCommand MergeAndExportFileCommand { get; set; }

        public string OutputFolderPath
        {
            get { return _outputFolderPath; }
            set
            {
                SetProperty( ref _outputFolderPath, value); 
                MergeAndExportFileCommand.RaiseCanExecuteChanged();
            }
        }

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
            _eventAggregator.GetEvent<FromMergerToExporterEvent>().Subscribe(FillFileToMergeList);
            SetupCommands();
            Headline = "Export .csv Datei";
        }

        #endregion

        #region Methods

        private void FillFileToMergeList(ObservableCollection<StatisticFile> filesToMerge)
        {
            StatisticFilesToMerge = filesToMerge;
        }

        private void SetupCommands()
        {
            OpenOutputFolderDialogCommand = new DelegateCommand(SelectOutputFolder);
            MergeAndExportFileCommand = new DelegateCommand(MergeAndExportCsvFile, CanMergeAndExportCsvFile);
        }

        private void SelectOutputFolder()
        {
            _folderBrowserDialog = new FolderBrowserDialog();
            _folderBrowserDialog.Title = "Ausgabeverzeichnis";
            _folderBrowserDialog.InitialFolder = @"D:\";
            _folderBrowserDialog.AllowMultiSelect = false;

            if (_folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                OutputFolderPath = _folderBrowserDialog.SelectedFolder;
            }
        }

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

        private void MergeAndExportCsvFile()
        {
            CsvFileMergerService.MergeAndCreateFile(ExportFileName, OutputFolderPath,StatisticFilesToMerge);
            MessageBox.Show($"Dateien wurden zusammengeführt und liegen unter: {OutputFolderPath}\\{ExportFileName}.csv",
                "Erfolgreicher Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion
    }
}
