/*
 * -----------------------------------------------------------------------------
 *	 
 *   Filename		:   CsvFileExporterModule.cs
 *   Date			:   2022-09-29
 *   All rights reserved
 * 
 * -----------------------------------------------------------------------------
 * @author     Patrick Robin <p.robin@smartperform.de>
 * @Version      1.0.0
 */

using CSVFileExporter.Views;
using CSVMerger.Core;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace CSVFileExporter
{
    /// <summary>
    /// Module File for Prism functionality
    /// </summary>
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