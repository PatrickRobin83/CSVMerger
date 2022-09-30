/*
*----------------------------------------------------------------------------------
*          Filename:	CollectionSorterService.cs
*          Date:        2022.10.01
*          All rights reserved
*
*----------------------------------------------------------------------------------
* @author Patrick Robin <support@rietrob.de>
*/

using CSVMerger.Core.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace CSVMerger.Core.Services
{
    public static class CollectionSorterService
    {
        #region Fields



        #endregion

        #region Properties

        #endregion

        #region Constructor

        #endregion

        #region Methods

        #endregion

        #region Commands
        public static ObservableCollection<StatisticFile> SortCollection(ObservableCollection<StatisticFile> statisticFiles)
        {
            ObservableCollection<StatisticFile> orderedCollection =
                new ObservableCollection<StatisticFile>(statisticFiles.OrderBy(file => file.CreationDate));

            return orderedCollection;
        }
        #endregion
    }
}