using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.IO;
using FolderBrowserEx;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Forms;
using CSVMerger.Core.Models;
using FolderBrowserDialog = FolderBrowserEx.FolderBrowserDialog;


namespace CSVFileImporter.ViewModels
{
    public class CsvFileImporterViewModel : BindableBase
    {
        private readonly IFolderBrowserDialog _folderBrowserDialog;
        private string _headLine;
        private string _statisticFolder;
        private ObservableCollection<StatisticFile> _statisticFiles = new ObservableCollection<StatisticFile>();
        private ObservableCollection<StatisticFile> _selectedStatisticFiles = new ObservableCollection<StatisticFile>();
        private StatisticFile _statisticFile;

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
        public DelegateCommand AddSelectedFilesCommand { get; set; }

        public string StatisticFolder
        {
            get { return _statisticFolder; }
            set { SetProperty(ref _statisticFolder, value); }
        }

        public CsvFileImporterViewModel()
        {
            HeadLine = "Datei Import";
            _folderBrowserDialog = new FolderBrowserDialog();
            SetupCommands();
        }

        private void SetupCommands()
        {
            OpenFolderSelectCommand = new DelegateCommand(OpenFolderSelect);
            AddSelectedFilesCommand = new DelegateCommand(AddSelectedFiles);
        }

        private void AddSelectedFiles()
        {
            SelectedStatisticFiles.Add(StatisticFile);
            StatisticFiles.Remove(StatisticFile);
        }

        private void OpenFolderSelect()
        {
            _folderBrowserDialog.Title = "Statistik Pfad";
            _folderBrowserDialog.InitialFolder = @"D:\Projekte\smartPerform\Test_SiemensStatistik\";
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
    }
}
