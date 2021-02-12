namespace Plisky.ClockChallenge.Test {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Plisky.Diagnostics;
    using Plisky.Test;
    using Xunit;

    /// <summary>
    /// Complete use cases - the end result of what we have been working toward.
    /// </summary>
    public class UseCaseTests {
        Bilge b = new Bilge(tl: System.Diagnostics.SourceLevels.Verbose);

        [Theory(DisplayName = nameof(FullUseCasesWork))]
        [Trait(Traits.Age, Traits.Integration)]
        [Trait(Traits.Style, Traits.Unit)]
        [InlineData(1, 30, "half past one")]
        [InlineData(1, 29, "one twenty-nine")]
        [InlineData(0, 15, "quarter past midnight")]
        [InlineData(1, 01, "one past one")]
        [InlineData(1, 3, "three past one")]
        [InlineData(1, 31, "one thirty-one")]
        [InlineData(0, 12, "twelve past midnight")]
        [InlineData(1, 45, "quarter to two")]
        [InlineData(1, 15, "quarter past one")]
        [InlineData(0, 0, "midnight")]
        [InlineData(12, 0, "noon")]
        public void FullUseCasesWork(int hour, int minute, string expected) {
            b.Info.Flow();

            var sut = new TalkingClockSupport();
            string reply = sut.ConvertTime(hour, minute);

            Assert.Equal(expected, reply);
        }


    }
}
