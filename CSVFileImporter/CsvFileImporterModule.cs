/*
 * -----------------------------------------------------------------------------
 *	 
 *   Filename		:   CsvFileImporterModule.cs
 *   Date			:   2022-09-29
 *   All rights reserved
 * 
 * -----------------------------------------------------------------------------
 * @author     Patrick Robin <p.robin@smartperform.de>
 * @Version      1.0.0
 */

using CSVFileImporter.Views;
using CSVMerger.Core;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace CSVFileImporter
{
    /// <summary>
    /// Module File for Prism functionality
    /// </summary>
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