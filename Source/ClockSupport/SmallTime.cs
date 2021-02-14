namespace Plisky.ClockChallenge {

    /// <summary>
    /// Represents the time as hour and minute.
    /// </summary>
    public struct SmallTime {
        public int Hour { get; set; }
        public int Minute { get; set; }

        public SmallTime(int hr, int mn) : this() {
            Hour = hr;
            Minute = mn;
        }
    }
}