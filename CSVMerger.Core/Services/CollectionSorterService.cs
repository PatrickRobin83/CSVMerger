/*
*----------------------------------------------------------------------------------
*          Filename:	CollectionSorterService.cs
*          Date:        2022.10.01
*
*          All rights reserved
* 
* -----------------------------------------------------------------------------
* @author     Patrick Robin <p.robin@smartperform.de>
* @Version      1.0.0
*/

using CSVMerger.Core.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace CSVMerger.Core.Services
{
    /// <summary>
    /// Sort a Observable Collection
    /// </summary>
    public static class CollectionSorterService
    {
        #region Fields
        #endregion

        #region Properties

        #endregion

        #region Constructor

        #endregion

        #region Methods
        /// <summary>
        /// Sorts a Observable Collection Ascending by Creation Date
        /// </summary>
        /// <param name="statisticFiles">Collection to sort</param>
        /// <returns>Sorted Observable Collection</returns>
        public static ObservableCollection<StatisticFile> SortCollection(ObservableCollection<StatisticFile> statisticFiles)
        {
            ObservableCollection<StatisticFile> orderedCollection =
                new ObservableCollection<StatisticFile>(statisticFiles.OrderBy(file => file.CreationDate));

            return orderedCollection;
        }
        #endregion

        #region Commands

        #endregion
    }
}