using System;
using Xunit;
using Humanizer.DateTimeHumanizeStrategy;
using Humanizer;
using Plisky.Diagnostics;
using Plisky.Test;

namespace Plisky.ClockChallenge.Test {
    public class Exploratory {
        Bilge b = new Bilge(tl: System.Diagnostics.SourceLevels.Verbose);
        public Exploratory() {

        }

        [Fact]
        public void ExploratoryTestOne() {
            21.ToOrdinalWords();
        }




        [Theory(DisplayName = nameof(BadParsesThrowException))]
        [Trait(Traits.Age, Traits.Fresh)]
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
        [Trait(Traits.Age, Traits.Fresh)]
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



        [Theory(DisplayName = nameof(FullUseCasesWork))]
        [Trait(Traits.Age, Traits.Fresh)]
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
