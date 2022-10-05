/*
 * -----------------------------------------------------------------------------
 *	 
 *   Filename		:   CsvFileImporterViewModel.cs
 *   Date			:   2022-09-29
 *   All rights reserved
 * 
 * -----------------------------------------------------------------------------
 * @author     Patrick Robin <p.robin@smartperform.de>
 * @Version      1.0.0
 */

using System;
using CSVMerger.Core.Events;
using CSVMerger.Core.Models;
using CSVMerger.Core.Services;
using FolderBrowserEx;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;
using CSVMerger.Core.Enums;
using FolderBrowserDialog = FolderBrowserEx.FolderBrowserDialog;


namespace CSVFileImporter.ViewModels
{
    /// <summary>
    /// ViewModel for CsvFileImporterView
    /// </summary>
    public class CsvFileImporterViewModel : BindableBase
    {
        #region Fields

        private IFolderBrowserDialog _folderBrowserDialog;
        private string _headLine;
        private string _statisticFolder;
        private ObservableCollection<StatisticFile> _statisticFiles = new ObservableCollection<StatisticFile>();
        private ObservableCollection<StatisticFile> _selectedFiles = new ObservableCollection<StatisticFile>();
        private IEventAggregator _eventAggregator;
        private bool _canAddExecute;
        private bool _canAddAllFiles;

        #endregion

        #region Properties

        /// <summary>
        /// Bool to determine the state of the button IsEnabled = true/false 
        /// </summary>
        public bool CanAddExecute
        {
            get { return _canAddExecute; }
            set
            {
                SetProperty(ref _canAddExecute, value);
            }
        }

        /// <summary>
        /// Headline for the Import section
        /// </summary>
        public string HeadLine
        {
            get { return _headLine; }
            set { SetProperty(ref _headLine, value); }
        }
        /// <summary>
        /// Collection of StatisticFiles which imported
        /// </summary>
        public ObservableCollection<StatisticFile> StatisticFiles
        {
            get { return _statisticFiles; }
            set
            {
                SetProperty(ref _statisticFiles, value);

            }
        }
        /// <summary>
        /// Collection of Selected Files in the ListView 
        /// </summary>
        public ObservableCollection<StatisticFile> SelectedFiles
        {
            get { return _selectedFiles; }
            set
            {
                SetProperty(ref _selectedFiles, value);
            }
        }

        /// <summary>
        /// Path of the files to be imported
        /// </summary>
        public string StatisticFolder
        {
            get { return _statisticFolder; }
            set { SetProperty(ref _statisticFolder, value); }
        }
        /// <summary>
        /// Command for the folder selection dialog  
        /// </summary>
        public DelegateCommand OpenFolderSelectCommand { get; set; }
        /// <summary>
        /// Command to add the selected entrys to the collection of files to merge
        /// </summary>
        public DelegateCommand AddSelectedFilesCommand { get; set; }
        /// <summary>
        /// Command to add all imported files to the collection of files to merge
        /// </summary>
        public DelegateCommand AddAllFilesCommand { get; set; }

        /// <summary>
        /// Bool to determine the state of the button IsEnabled = true/false 
        /// </summary>
        public bool CanAddAllFiles
        {
            get { return _canAddAllFiles; }
            set { SetProperty(ref _canAddAllFiles, value); }
        }
        /// <summary>
        /// Helper Property to add the multiple selection to the collection of files to merge 
        /// </summary>
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
            _eventAggregator.GetEvent<ClearCollectionsEvent>().Subscribe(ClearFileCollections);
            InitializeCommands();
        }

        #endregion

        #region Methods
        /// <summary>
        /// Initializes all commands
        /// </summary>
        private void InitializeCommands()
        {
            OpenFolderSelectCommand = new DelegateCommand(SelectImportFolder);
            AddSelectedFilesCommand = new DelegateCommand(AddSelectedFiles, CanAddSelectFile);
            AddAllFilesCommand = new DelegateCommand(AddAllFiles, CanAddAllFilesExecute);
        }
        /// <summary>
        /// Checks whether the button AddAllFiles can be pressed or whether it must be deactivated.
        /// </summary>
        /// <returns>bool</returns>
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
        /// <summary>
        /// Adds all files from the imported files collection to the merge file collection
        /// </summary>
        private void AddAllFiles()
        {
            SelectedFiles = new ObservableCollection<StatisticFile>(StatisticFiles);
            AddSelectedFiles();
            StatisticFiles.Clear();
            SelectedFiles.Clear();
            AddAllFilesCommand.RaiseCanExecuteChanged();
            AddSelectedFilesCommand.RaiseCanExecuteChanged();
        }
        /// <summary>
        /// Adds the selected files from the imported files collection to the merge file collection
        /// </summary>
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
            AddAllFilesCommand.RaiseCanExecuteChanged();
            AddSelectedFilesCommand.RaiseCanExecuteChanged();


        }
        /// <summary>
        /// Adds the given file to the collection of imported files
        /// </summary>
        /// <param name="statisticFile"></param>
        private void AddFileToStatisticFileList(StatisticFile statisticFile)
        {
            StatisticFiles.Add(statisticFile);
            if (StatisticFiles.Count > 1)
            {
                AddAllFilesCommand.RaiseCanExecuteChanged();
            }
            AddSelectedFilesCommand.RaiseCanExecuteChanged();
        }
        /// <summary>
        /// Checks whether the button AddAllFiles can be pressed or whether it must be deactivated.
        /// </summary>
        /// <returns>bool</returns>
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
        /// <summary>
        /// Opens a Folder select dialog and sets the path from Property StatisticFolder
        /// </summary>
        private void SelectImportFolder()
        {
            try
            {
                _folderBrowserDialog = new FolderBrowserDialog();
                _folderBrowserDialog.Title = "Statistik Pfad";
                _folderBrowserDialog.InitialFolder = @"C:\";
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
            catch (Exception ex)
            {
                Logger.Log(LogState.Error, ex.Message);
            }

        }
        /// <summary>
        /// Clears the Collections StatisticFiles and SelectedFiles
        /// </summary>
        private void ClearFileCollections()
        {
            ClearCollections.ClearCollection(StatisticFiles);
            ClearCollections.ClearCollection(SelectedFiles);
            AddAllFilesCommand.RaiseCanExecuteChanged();
            AddSelectedFilesCommand.RaiseCanExecuteChanged();
        }

        #endregion
    }
}
