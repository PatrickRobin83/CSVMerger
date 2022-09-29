using CSVMerger.Core.Models;
using FolderBrowserEx;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;
using CSVMerger.Core.Events;
using Prism.Events;
using FolderBrowserDialog = FolderBrowserEx.FolderBrowserDialog;
using System.IO.Compression;


namespace CSVFileImporter.ViewModels
{
    public class CsvFileImporterViewModel : BindableBase
    {
        #region Fields
        private IFolderBrowserDialog _folderBrowserDialog;
        private string _headLine;
        private string _statisticFolder;
        private ObservableCollection<StatisticFile> _statisticFiles = new ObservableCollection<StatisticFile>();
        private ObservableCollection<StatisticFile> _selectedStatisticFiles = new ObservableCollection<StatisticFile>();
        private StatisticFile _statisticFile;
        private IEventAggregator _eventAggregator;
        #endregion

        #region Properties
        public StatisticFile StatisticFile
        {
            get { return _statisticFile; }
            set { SetProperty(ref _statisticFile, value); }
        }
        public string HeadLine
        {
            get { return _headLine; }
            set { SetProperty(ref _headLine, value); }
        }

        public ObservableCollection<StatisticFile> StatisticFiles
        {
            get { return _statisticFiles; }
            set { SetProperty(ref _statisticFiles, value); }
        }
        public ObservableCollection<StatisticFile> SelectedStatisticFiles
        {
            get { return _selectedStatisticFiles; }
            set { SetProperty(ref _selectedStatisticFiles, value); }
        }
        public DelegateCommand OpenFolderSelectCommand { get; set; }
        public DelegateCommand AddSelectedFileCommand { get; set; }

        public string StatisticFolder
        {
            get { return _statisticFolder; }
            set { SetProperty(ref _statisticFolder, value); }
        }

        #endregion

        #region Constructor

        public CsvFileImporterViewModel(IEventAggregator eventAggregator)
        {
            HeadLine = "Datei Import";
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<FromMergerToImporterEvent>().Subscribe(AddFileToStatisticFileList);
            SetupCommands();
        }
        #endregion

        #region Methods
        private void SetupCommands()
        {
            OpenFolderSelectCommand = new DelegateCommand(OpenFolderSelect);
            AddSelectedFileCommand = new DelegateCommand(AddSelectedFile);
        }

        private void AddSelectedFile()
        {
            _eventAggregator.GetEvent<FromImporterToMergerEvent>().Publish(StatisticFile);
            StatisticFiles.Remove(StatisticFile);
        }

        private void AddFileToStatisticFileList(StatisticFile statisticFile)
        {
            StatisticFiles.Add(statisticFile);
        }


        private void OpenFolderSelect()
        {
            _folderBrowserDialog = new FolderBrowserDialog();
            _folderBrowserDialog.Title = "Statistik Pfad";
            _folderBrowserDialog.InitialFolder = @"D:\";
            _folderBrowserDialog.AllowMultiSelect = false;
            if (_folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                StatisticFolder = _folderBrowserDialog.SelectedFolder;
            }
            StatisticFiles.Clear();
            foreach (string file in Directory.GetFiles(StatisticFolder))
            {
                if (Path.GetExtension(file) == ".csv")
                {
                    StatisticFile tempFile = new StatisticFile();
                    tempFile.Name = Path.GetFileName(file);
                    tempFile.Path = Path.GetFullPath(file);
                    tempFile.Extension = Path.GetExtension(file);
                    StatisticFiles.Add(tempFile);
                }
            }

        } 
        #endregion
    }
}
