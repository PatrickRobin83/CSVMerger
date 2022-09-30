using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSVMerger.Core.Events;
using CSVMerger.Core.Models;
using CSVMerger.Core.Services;
using Prism.Events;

namespace CSVFileMerger.ViewModels
{
    public class CsvFileMergerViewModel : BindableBase
    {
        #region Fields
        private string _headline;
        private IEventAggregator _eventAggregator;
        private ObservableCollection<StatisticFile> _filesToMerge = new ObservableCollection<StatisticFile>();
        private StatisticFile _selectedFileToRemove;
        private bool _canRemoveExecute = false;
        #endregion

        #region Properties

        public StatisticFile SelectedFileToRemove
        {
            get { return _selectedFileToRemove; }
            set
            {
                SetProperty(ref _selectedFileToRemove, value); 
                RemoveFileCommand.RaiseCanExecuteChanged();
            }

        }

        public string Headline
        {
            get { return _headline; }
            set { SetProperty(ref _headline, value); }
        }

        public ObservableCollection<StatisticFile> FilesToMerge
        {
            get { return _filesToMerge; }
            set
            {
                SetProperty(ref _filesToMerge, value);
            }
        }

        public DelegateCommand RemoveFileCommand { get; set; }

        public bool CanRemoveExecute
        {
            get { return _canRemoveExecute; }
            set
            {
                SetProperty(ref _canRemoveExecute, value);
            }
        }

        #endregion

        #region Constructor
        public CsvFileMergerViewModel(IEventAggregator eventAggregator)
        {
            Headline = "Dateien zusammenführen";
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<FromImporterToMergerEvent>().Subscribe(AddFileToFilesToMerge);
            SetupCommands();
        }

        #endregion

        #region Methods
        private void AddFileToFilesToMerge(StatisticFile fileToAdd)
        {
            FilesToMerge.Add(fileToAdd);
            if (FilesToMerge.Count > 1)
            {
                FilesToMerge = CollectionSorterService.SortCollection(FilesToMerge);
                _eventAggregator.GetEvent<FromMergerToExporterEvent>().Publish(FilesToMerge);
            }
        }

        private void SetupCommands()
        {
            RemoveFileCommand = new DelegateCommand(RemoveFileFromMergeList,CanRemoveFileExecute);
        }

        private void RemoveFileFromMergeList()
        {
            _eventAggregator.GetEvent<FromMergerToImporterEvent>().Publish(SelectedFileToRemove);
            FilesToMerge.Remove(SelectedFileToRemove);
            if (FilesToMerge.Count > 1)
            {
                _eventAggregator.GetEvent<FromMergerToExporterEvent>().Publish(FilesToMerge);
            }
        }

        private bool CanRemoveFileExecute()
        {
            if (SelectedFileToRemove != null && !string.IsNullOrEmpty(SelectedFileToRemove.Name))
            {
                CanRemoveExecute = true;
            }
            else
            {
                CanRemoveExecute = false;
            }

            return CanRemoveExecute;
        }

        #endregion
    }
}
