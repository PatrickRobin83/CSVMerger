/*
*----------------------------------------------------------------------------------
*          Filename:	ClearCollections.cs
*          Date:        2022.10.01
*          All rights reserved
*
*----------------------------------------------------------------------------------
* @author Patrick Robin <support@rietrob.de>
*/

using System.Collections.ObjectModel;

namespace CSVMerger.Core.Services
{
    /// <summary>
    /// Clears a Observable Collection
    /// </summary>
    public static class ClearCollections
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructor

        #endregion

        #region Methods
        /// <summary>
        /// Clears the given Observable Collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">Observable Collection to clear</param>
        public static void ClearCollection<T>(ObservableCollection<T> collection)
        {
            collection.Clear();
        }
        #endregion

        #region Commands

        #endregion
    }
}