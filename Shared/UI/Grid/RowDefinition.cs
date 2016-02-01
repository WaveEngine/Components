﻿#region File Description
//-----------------------------------------------------------------------------
// RowDefinition
//
// Copyright © 2015 Wave Engine S.L. All rights reserved.
// Use is subject to license terms.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements

using System.Runtime.Serialization;
#endregion

namespace WaveEngine.Components.UI
{
    /// <summary>
    /// Defines row-specific properties that apply to Grid elements.
    /// </summary>
    [DataContract(Namespace = "WaveEngine.Components.UI")]
    public sealed class RowDefinition
    {
        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public GridLength Height { get; set; }

        /// <summary>
        /// Gets the actual height.
        /// </summary>
        /// <value>
        /// The actual height.
        /// </value>
        public float ActualHeight { get; internal set; }
    }
}