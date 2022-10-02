/*
 * -----------------------------------------------------------------------------
 *	 
 *   Filename		:   CsvFileMergerViewModel.cs
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
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace CSVFileMerger.ViewModels
{
    /// <summary>
    /// ViewModel for CsvFileMergerView
    /// </summary>
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
        /// <summary>
        /// File which is selected to remove
        /// </summary>
        public StatisticFile SelectedFileToRemove
        {
            get { return _selectedFileToRemove; }
            set
            {
                SetProperty(ref _selectedFileToRemove, value); 
                RemoveFileCommand.RaiseCanExecuteChanged();
            }

        }
        /// <summary>
        /// Headline for the merge section
        /// </summary>
        public string Headline
        {
            get { return _headline; }
            set { SetProperty(ref _headline, value); }
        }
        /// <summary>
        /// Collection of files which will merged
        /// </summary>
        public ObservableCollection<StatisticFile> FilesToMerge
        {
            get { return _filesToMerge; }
            set
            {
                SetProperty(ref _filesToMerge, value);
            }
        }
        /// <summary>
        /// Command for the Remove Button
        /// </summary>
        public DelegateCommand RemoveFileCommand { get; set; }

        /// <summary>
        /// Bool to determine the state of the button IsEnabled = true/false 
        /// </summary>
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
            _eventAggregator.GetEvent<ClearCollectionsEvent>().Subscribe(ClearMergeFilesCollection);
            InitializeCommands();
        }

        #endregion

        #region Methods
        /// <summary>
        /// Add a file to the College of files to merge
        /// </summary>
        /// <param name="fileToAdd">Selected file from Listbox</param>
        private void AddFileToFilesToMerge(StatisticFile fileToAdd)
        {
            FilesToMerge.Add(fileToAdd);
            if (FilesToMerge.Count > 1)
            {
                FilesToMerge = CollectionSorterService.SortCollection(FilesToMerge);
                _eventAggregator.GetEvent<FromMergerToExporterEvent>().Publish(FilesToMerge);
            }
        }
        /// <summary>
        /// Initializes Commands 
        /// </summary>
        private void InitializeCommands()
        {
            RemoveFileCommand = new DelegateCommand(RemoveFileFromMergeList,CanRemoveFileExecute);
        }

        /// <summary>
        /// Removes the selected StatisticFile from merge list and publishes it to the Importer and to the Exporter
        /// </summary>
        private void RemoveFileFromMergeList()
        {
            _eventAggregator.GetEvent<FromMergerToImporterEvent>().Publish(SelectedFileToRemove);
            FilesToMerge.Remove(SelectedFileToRemove);
            if (FilesToMerge.Count > 1)
            {
                _eventAggregator.GetEvent<FromMergerToExporterEvent>().Publish(FilesToMerge);
            }
        }
        /// <summary>
        /// Checks whether the button can be pressed or whether it must be deactivated.
        /// </summary>
        /// <returns>bool</returns>
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
        /// <summary>
        /// Clears the Observable Collection
        /// </summary>
        private void ClearMergeFilesCollection()
        {
            ClearCollections.ClearCollection(FilesToMerge);
        }

        #endregion
    }
}
