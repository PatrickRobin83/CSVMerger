using CSVFileMerger.ViewModels;
using CSVFileMerger.Views;
using CSVMerger.Core;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;

namespace CSVFileMerger
{
    public class CsvFileMergerModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public CsvFileMergerModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion(RegionNames.FileMergeRegion, typeof(CsvFileMergerView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}