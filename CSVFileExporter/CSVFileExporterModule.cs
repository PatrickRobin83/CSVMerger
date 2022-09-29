using CSVFileExporter.Views;
using CSVMerger.Core;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace CSVFileExporter
{
    public class CSVFileExporterModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public CSVFileExporterModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion(RegionNames.FileOutputRegion, typeof(CsvFileExporterView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}