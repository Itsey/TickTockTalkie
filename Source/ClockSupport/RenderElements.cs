namespace Plisky.ClockChallenge {

    /// <summary>
    /// There are four parts of text that can be modified pre-post are before and after, then minutes and hours and the bit in the middle.
    /// </summary>
    public enum RenderElements {
        Preamble,
        Postamble,
        MinuteText,
        HourText,
        Intermediary
    }
}