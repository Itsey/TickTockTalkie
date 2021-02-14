namespace Plisky.ClockChallenge.Test {

    using System;
    using Plisky.Diagnostics;
    using Plisky.Test;
    using Xunit;

    public class BasicRegression {
        private Bilge b = new Bilge(tl: System.Diagnostics.SourceLevels.Verbose);
        private const string ORIGNAL_TEXT = "bannana";
        private const string REPLACEMENT_TEXT = "orange";

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

            var sut = new TalkingClockSupport(ConverterChainBase.GetFullChain());

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

            var sut = new TalkingClockSupport(ConverterChainBase.GetFullChain());
            var result = sut.ParseTime(text);

            Assert.Equal(expectedMins, result.Minute);
            Assert.Equal(expectedHours, result.Hour);
        }

        [Fact(DisplayName = nameof(SmallTimeRendererDefaultWorks))]
        [Trait(Traits.Age, Traits.Regression)]
        [Trait(Traits.Style, Traits.Unit)]
        public void SmallTimeRendererDefaultWorks() {
            b.Info.Flow();
            var sut = new SmallTimeRenderer(new SmallTime(1, 1));

            sut.DefaultRenderText(RenderElements.HourText, ORIGNAL_TEXT);
            sut.DefaultRenderText(RenderElements.HourText, REPLACEMENT_TEXT);

            Assert.Equal(ORIGNAL_TEXT, sut.GetRenderText(RenderElements.HourText));
        }

        [Fact(DisplayName = nameof(SmallTimeRendererForceWorks))]
        [Trait(Traits.Age, Traits.Regression)]
        [Trait(Traits.Style, Traits.Unit)]
        public void SmallTimeRendererForceWorks() {
            b.Info.Flow();
            var sut = new SmallTimeRenderer(new SmallTime(1, 1));

            sut.DefaultRenderText(RenderElements.HourText, ORIGNAL_TEXT);
            sut.ForceRenderText(RenderElements.HourText, REPLACEMENT_TEXT);

            Assert.Equal(REPLACEMENT_TEXT, sut.GetRenderText(RenderElements.HourText));
        }
    }
}