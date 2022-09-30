using System;
using System.Collections.Generic;
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
using System.Linq;
using CSVMerger.Core.Services;


namespace CSVFileImporter.ViewModels
{
    public class CsvFileImporterViewModel : BindableBase
    {
        #region Fields

        private IFolderBrowserDialog _folderBrowserDialog;
        private string _headLine;
        private string _statisticFolder;
        private ObservableCollection<StatisticFile> _statisticFiles = new ObservableCollection<StatisticFile>();
        private ObservableCollection<StatisticFile> _selectedFiles = new ObservableCollection<StatisticFile>();
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
                AddSelectedFilesCommand.RaiseCanExecuteChanged();
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

        public ObservableCollection<StatisticFile> SelectedFiles
        {
            get { return _selectedFiles; }
            set
            {
                SetProperty(ref _selectedFiles, value);
            }
        }

        public string StatisticFolder
        {
            get { return _statisticFolder; }
            set { SetProperty(ref _statisticFolder, value); }
        }

        public DelegateCommand OpenFolderSelectCommand { get; set; }
        public DelegateCommand AddSelectedFilesCommand { get; set; }
        public DelegateCommand AddAllFilesCommand { get; set; }

        public bool CanAddAllFiles
        {
            get { return _canAddAllFiles; }
            set { SetProperty(ref _canAddAllFiles, value); }
        }

        public System.Collections.IList SelectedItems
        {
            get { return SelectedFiles; }
            set
            {
                SelectedFiles.Clear();
                foreach (StatisticFile statisticFile in value)
                {
                    SelectedFiles.Add(statisticFile);
                }
                AddSelectedFilesCommand.RaiseCanExecuteChanged();
            }
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
            AddSelectedFilesCommand = new DelegateCommand(AddSelectedFiles, CanAddSelectFile);
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
            //ObservableCollection<StatisticFile> tmpList = new ObservableCollection<StatisticFile>(StatisticFiles);
            SelectedFiles = StatisticFiles;
            AddSelectedFiles();
            StatisticFiles.Clear();
            SelectedFiles.Clear();
            AddAllFilesCommand.RaiseCanExecuteChanged();
        }

        private void AddSelectedFiles()
        {
            ObservableCollection<StatisticFile> tmp = new ObservableCollection<StatisticFile>(SelectedFiles);
            foreach (StatisticFile statisticFile in tmp)
            {
                _eventAggregator.GetEvent<FromImporterToMergerEvent>().Publish(statisticFile);
                StatisticFiles.Remove(statisticFile);
            }
            SelectedFiles.Clear();
            tmp.Clear();

        }
        //private void AddSelectedFiles(StatisticFile file)
        //{
        //    _eventAggregator.GetEvent<FromImporterToMergerEvent>().Publish(file);
        //    //StatisticFiles.Remove(file);
        //}

        private void AddFileToStatisticFileList(StatisticFile statisticFile)
        {
            StatisticFiles.Add(statisticFile);
        }

        private bool CanAddSelectFile()
        {
            if (SelectedFiles.Count > 0)
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
                    tempFile.CreationDate = File.GetCreationTime(Path.GetFullPath(file));
                    StatisticFiles.Add(tempFile);
                }
            }

            StatisticFiles = CollectionSorterService.SortCollection(StatisticFiles);

            AddAllFilesCommand.RaiseCanExecuteChanged();

        }

        #endregion
    }
}
