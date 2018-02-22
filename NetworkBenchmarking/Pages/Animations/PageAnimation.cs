﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkBenchmarking
{
  /// <summary>
  /// Styles of page transition animations
  /// </summary>
  public enum PageAnimation
  {
    /// <summary>
    /// Default no animation
    /// </summary>
    None = 0,

    /// <summary>
    /// </summary>
    SlideAndFadeInFromRight = 1,

    /// <summary>
    /// </summary>
    SlideAndFadeOutToLeft = 2,

    /// <summary>
    /// </summary>
    SlideAndFadeInFromLeft = 3,

    /// <summary>
    /// </summary>
    SlideAndFadeOutToRight = 4
  }
}
