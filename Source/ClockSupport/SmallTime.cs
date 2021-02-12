using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Plisky.ClockChallenge {

    /// <summary>
    /// Small time is just hours and minutes and some info on formatting, way less complex than a timespan.
    /// </summary>
    [DebuggerDisplay("{Hour}:{Minute}")]
    public class SmallTime {
        public int Hour { get; set; }
        public int Minute { get; set; }       

    }
}
