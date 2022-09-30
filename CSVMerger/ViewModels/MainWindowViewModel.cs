using System.Reflection.Metadata;
using CSVFileExporter.ViewModels;
using CSVFileExporter.Views;
using CSVFileImporter.ViewModels;
using CSVFileImporter.Views;
using CSVFileMerger.ViewModels;
using CSVFileMerger.Views;
using FolderBrowserEx;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace CSVMerger.ViewModels
{
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
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public int WindowHeight
        {
            get { return _windowHeight; }
            set
            {
                SetProperty(ref _windowHeight, value);
            }
        }
        public int WindowWidth
        {
            get { return _windowWidth; }
            set
            {
                SetProperty(ref _windowWidth, value);
            }
        }
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
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            SetupViews();
        }
        #endregion

        #region Methods
        public void SetupViews()
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
