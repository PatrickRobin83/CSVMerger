/*
 * -----------------------------------------------------------------------------
 *	 
 *   Filename		:   FromImporterToMergerEvent.cs
 *   Date			:   2022-09-30 09:01:35
 *   All rights reserved
 * 
 * -----------------------------------------------------------------------------
 * @author     Patrick Robin <p.robin@smartperform.de>
 * @Version      1.0.0
 */

using System;
using CSVMerger.Core.Models;
using Prism.Events;

namespace CSVMerger.Core.Events
{
    /// <summary>
    /// Event to publish the statistic file from Importer to Merger
    /// </summary>
    public class FromImporterToMergerEvent : PubSubEvent<StatisticFile>
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

        #endregion
    }
}