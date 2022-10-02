/*
 * -----------------------------------------------------------------------------
 *	 
 *   Filename		:   CsvFileMergerModule.cs
 *   Date			:   2022-09-29
 *   All rights reserved
 * 
 * -----------------------------------------------------------------------------
 * @author     Patrick Robin <p.robin@smartperform.de>
 * @Version      1.0.0
 */

using CSVFileMerger.Views;
using CSVMerger.Core;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace CSVFileMerger
{
    /// <summary>
    /// Module File for Prism functionality
    /// </summary>
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