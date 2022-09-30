using System;
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
        private StatisticFile _statisticFile;
        private IEventAggregator _eventAggregator;
        private bool _canAddExecute;
        private bool _canAddAllFiles;

        #endregion

        #region Properties

        public bool CanAddExecute
        {
            get { return _canAddExecute; }
            set
            {
                SetProperty(ref _canAddExecute, value);
            }
        }

        public StatisticFile StatisticFile
        {
            get { return _statisticFile; }
            set
            {
                SetProperty(ref _statisticFile, value);
                AddSelectedFileCommand.RaiseCanExecuteChanged();
            }
        }
        public string HeadLine
        {
            get { return _headLine; }
            set { SetProperty(ref _headLine, value); }
        }

        public ObservableCollection<StatisticFile> StatisticFiles
        {
            get { return _statisticFiles; }
            set
            {
                SetProperty(ref _statisticFiles, value);
            }
        }

        public string StatisticFolder
        {
            get { return _statisticFolder; }
            set { SetProperty(ref _statisticFolder, value); }
        }

        public DelegateCommand OpenFolderSelectCommand { get; set; }
        public DelegateCommand AddSelectedFileCommand { get; set; }
        public DelegateCommand AddAllFilesCommand { get; set; }

        public bool CanAddAllFiles
        {
            get { return _canAddAllFiles; }
            set { SetProperty(ref _canAddAllFiles, value); }
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
            AddSelectedFileCommand = new DelegateCommand(AddSelectedFile, CanAddSelectFile);
            AddAllFilesCommand = new DelegateCommand(AddAllFiles, CanAddAllFilesExecute);
        }

        private bool CanAddAllFilesExecute()
        {
            if (StatisticFiles.Count > 0)
            {
                CanAddAllFiles = true;
            }
            else
            {
                CanAddAllFiles = false;
            }

            return CanAddAllFiles;
        }

        private void AddAllFiles()
        {
            ObservableCollection<StatisticFile> tmpList = new ObservableCollection<StatisticFile>(StatisticFiles);
            
            foreach (StatisticFile statisticFile in tmpList)
            {
                AddSelectedFile(statisticFile);
            }

            StatisticFiles.Clear();
            AddAllFilesCommand.RaiseCanExecuteChanged();
        }

        private void AddSelectedFile()
        {
            _eventAggregator.GetEvent<FromImporterToMergerEvent>().Publish(StatisticFile);
            StatisticFiles.Remove(StatisticFile);
            StatisticFile = null;
        }
        private void AddSelectedFile(StatisticFile file)
        {
            _eventAggregator.GetEvent<FromImporterToMergerEvent>().Publish(file);
            //StatisticFiles.Remove(file);
        }

        private void AddFileToStatisticFileList(StatisticFile statisticFile)
        {
            StatisticFiles.Add(statisticFile);
        }

        private bool CanAddSelectFile()
        {
            if (StatisticFile != null && !String.IsNullOrEmpty(StatisticFile.Name))
            {
                CanAddExecute = true;
            }
            else
            {
                CanAddExecute = false;
            }
            return CanAddExecute;
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
            AddAllFilesCommand.RaiseCanExecuteChanged();

        } 

        #endregion
    }
}
