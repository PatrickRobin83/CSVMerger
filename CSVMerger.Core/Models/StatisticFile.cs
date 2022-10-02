/*
 * -----------------------------------------------------------------------------
 *	 
 *   Filename		:   StatisticFile.cs
 *   Date			:   2022-09-29 16:01:21
 *   All rights reserved
 * 
 * -----------------------------------------------------------------------------
 * @author     Patrick Robin <p.robin@smartperform.de>
 * @Version      1.0.0
 */

using System;

namespace CSVMerger.Core.Models
{
    /// <summary>
    /// Represents a statistic file
    /// </summary>
    public class StatisticFile
    {
        #region Fields
        #endregion

        #region Properties

        public string Name { get; set; }
        public string Path { get; set; }
        public string Extension { get; set; }
        public DateTime CreationDate { get; set; }

        #endregion

        #region Constructor

        #endregion

        #region Methods

        public override string ToString()
        {
            return Name;
        }

        #endregion

    }
}