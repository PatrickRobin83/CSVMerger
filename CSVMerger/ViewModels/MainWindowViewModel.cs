/*
 * -----------------------------------------------------------------------------
 *	 
 *   Filename		:   MainWindowViewModel.cs
 *   Date			:   2022-09-29 09:12:01
 *   All rights reserved
 * 
 * -----------------------------------------------------------------------------
 * @author     Patrick Robin <p.robin@smartperform.de>
 * @Version      1.0.0
 */


using CSVFileExporter.ViewModels;
using CSVFileExporter.Views;
using CSVFileImporter.ViewModels;
using CSVFileImporter.Views;
using CSVFileMerger.ViewModels;
using CSVFileMerger.Views;
using CSVMerger.Core.Enums;
using CSVMerger.Core.Services;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace CSVMerger.ViewModels
{
    /// <summary>
    /// ViewModel for the Main View / Main Window
    /// </summary>
    public class MainWindowViewModel : BindableBase
    {
        #region Fields
        /// <summary>
        /// RegionManager for Prism functionality
        /// </summary>
        private readonly IRegionManager _regionManager;
        private string _title = "CSVMerger";
        private int _windowHeight = 400;
        private int _windowWidth = 940;
        private string _resizeMode = "NoResize";
        private readonly IEventAggregator _eventAggregator;



        #endregion

        #region Properties
        /// <summary>
        /// Title Main Window
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        /// <summary>
        /// Main Window Height
        /// </summary>
        public int WindowHeight
        {
            get { return _windowHeight; }
            set
            {
                SetProperty(ref _windowHeight, value);
            }
        }
        /// <summary>
        /// Main Window Width
        /// </summary>
        public int WindowWidth
        {
            get { return _windowWidth; }
            set
            {
                SetProperty(ref _windowWidth, value);
            }
        }
        /// <summary>
        /// Defines the Resize Mode for the Main Window
        /// </summary>
        public string ResizeMode
        {
            get { return _resizeMode; }
            set
            {
                SetProperty(ref _resizeMode,value);
            }
        }

        #endregion

        #region Constructor
        public MainWindowViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            Logger.Log(LogState.Info, "Application Start");
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            SetupViewsAndDataContextForTheViews();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Sets up all views an their DataContext
        /// </summary>
        public void SetupViewsAndDataContextForTheViews()
        {
            CsvFileMergerView csvFileMergerView = new CsvFileMergerView
            {
                DataContext = new CsvFileMergerViewModel(_eventAggregator)
            };
            CsvFileImporterView csvFileImporterView = new CsvFileImporterView()
            {
                DataContext = new CsvFileImporterViewModel(_eventAggregator)
            };
            CsvFileExporterView csvFileExporterView = new CsvFileExporterView()
            {
                DataContext = new CsvFileExporterViewModel(_eventAggregator)
            };

        }

        #endregion
    }
}
