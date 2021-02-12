namespace Plisky.ClockChallenge.Test {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Plisky.Diagnostics;
    using Plisky.Test;
    using Xunit;

    public class BasicRegression {
        Bilge b = new Bilge(tl: System.Diagnostics.SourceLevels.Verbose);

        [Theory(DisplayName = nameof(BadParsesThrowException))]
        [Trait(Traits.Age, Traits.Regression)]
        [Trait(Traits.Style, Traits.Unit)]
        [InlineData("monkeyfish")]
        [InlineData("")]
        [InlineData("-12345")]
        [InlineData("-0:00")]
        [InlineData("0.00.00")]
        [InlineData("[]{}--!")]
        [InlineData(null)]
        [InlineData("-1:11")]
        public void BadParsesThrowException(string input) {
            b.Info.Flow();

            var sut = new TalkingClockSupport();

            Assert.Throws<ArgumentOutOfRangeException>(() => {
                _ = sut.ParseTime(input);
            });
        }


        [Theory(DisplayName = nameof(ParseInputAsTime))]
        [Trait(Traits.Age, Traits.Regression)]
        [Trait(Traits.Style, Traits.Unit)]
        [InlineData("1:11", 1, 11)]
        [InlineData("0", 0, 0)]
        [InlineData("12", 12, 0)]
        [InlineData("12:00", 12, 0)]
        [InlineData("13:59", 13, 59)]
        [InlineData("23:59", 23, 59)]
        public void ParseInputAsTime(string text, int expectedHours, int expectedMins) {
            b.Info.Flow();

            var sut = new TalkingClockSupport();
            var result = sut.ParseTime(text);

            Assert.Equal(expectedMins, result.Minute);
            Assert.Equal(expectedHours, result.Hour);
        }





    }
}
