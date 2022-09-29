using CSVFileImporter.Views;
using CSVMerger.Core;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace CSVFileImporter
{
    public class CsvFileImporterModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public CsvFileImporterModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion(RegionNames.FileImportRegion, typeof(CsvFileImporterView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}