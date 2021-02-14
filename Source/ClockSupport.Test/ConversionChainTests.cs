namespace Plisky.ClockChallenge.Test {

    using Plisky.Diagnostics;
    using Plisky.Test;
    using Xunit;

    public class ConversionChainTests {
        private Bilge b = new Bilge(tl: System.Diagnostics.SourceLevels.Verbose);

        [Theory(DisplayName = nameof(DefaultWords_BasicTest))]
        [Trait(Traits.Age, Traits.Regression)]
        [Trait(Traits.Style, Traits.Unit)]
        [InlineData(1, 12, "one", "twelve")]
        [InlineData(0, 0, "zero", "zero")]
        [InlineData(24, 15, "twenty-four", "fifteen")]
        [InlineData(23, 1, "twenty-three", "one")]
        public void DefaultWords_BasicTest(int hr, int min, string expMin, string expHour) {
            b.Info.Flow();

            var st = new SmallTime() {
                Hour = hr,
                Minute = min
            };

            var sut = new DefaultNumbersToWords();
            var res = sut.Handle(new SmallTimeRenderer(st));
            string rend = res.Render();

            Assert.Contains(expMin, rend);
            Assert.Contains(expHour, rend);
        }


        [Theory(DisplayName = nameof(MidnightAndNoonBasicTest))]
        [Trait(Traits.Age, Traits.Regression)]
        [Trait(Traits.Style, Traits.Unit)]
        [InlineData(1, 12, "one")]
        [InlineData(0, 0, "midnight")]
        [InlineData(12, 0, "noon")]
        [InlineData(24, 15, "midnight")]
        [InlineData(23, 1, "twenty-three")]
        public void MidnightAndNoonBasicTest(int hr, int min, string expected) {
            b.Info.Flow();

            var st = new SmallTime() {
                Hour = hr,
                Minute = min
            };

            var sut = ConverterChainBase.GetChain(new MidnightCapability(), new DefaultNumbersToWords());
            var res = sut.Handle(new SmallTimeRenderer(st));

            Assert.Contains(expected, res.Render());
        }

        [Theory(DisplayName = nameof(QuarterAndHalfCapability))]
        [Trait(Traits.Age, Traits.Regression)]
        [Trait(Traits.Style, Traits.Unit)]
        [InlineData(1, 12, "twelve")]
        [InlineData(0, 3, "three")]
        [InlineData(12, 45, "quarter")]
        [InlineData(24, 15, "quarter")]
        [InlineData(23, 1, "one")]
        [InlineData(23, 30, "half")]
        [InlineData(1, 0, "one")]
        [InlineData(1, 0, "exactly")]
        public void QuarterAndHalfCapability(int hr, int min, string expected) {
            b.Info.Flow();

            var st = new SmallTime() {
                Hour = hr,
                Minute = min
            };

            var sut = ConverterChainBase.GetChain(new QuarterHalfAndExact(), new DefaultNumbersToWords());
            var res = sut.Handle(new SmallTimeRenderer(st));

            Assert.Contains(expected, res.Render());
        }

        [Theory(DisplayName = nameof(PastCapabilityBasicTest))]
        [Trait(Traits.Age, Traits.Regression)]
        [Trait(Traits.Style, Traits.Unit)]
        [InlineData(1, 12, "twelve past")]
        [InlineData(0, 3, "three past")]
        [InlineData(12, 45, "fifteen to")]    // Learn something new every day its forty not fourty.
        [InlineData(24, 15, "fifteen past")]
        [InlineData(23, 1, "one past")]
        public void PastCapabilityBasicTest(int hr, int min, string expected) {
            b.Info.Flow();

            var st = new SmallTime() {
                Hour = hr,
                Minute = min
            };

            var sut = ConverterChainBase.GetChain(new PastAndToCapability(), new DefaultNumbersToWords());
            var res = sut.Handle(new SmallTimeRenderer(st));

            Assert.Contains(expected, res.Render());
        }

        [Theory(DisplayName = nameof(PastAndTwo_ModifiesMinutesWhenTo))]
        [Trait(Traits.Age, Traits.Regression)]
        [Trait(Traits.Style, Traits.Unit)]
        [InlineData(1, 54, 2, 6)]   // One fifty-four becomes 7 to two
        [InlineData(0, 0, 0, 0)]
        [InlineData(1, 30, 1, 30)]
        [InlineData(1, 31, 2, 29)]
        [InlineData(23, 54, 24, 6)]
        [InlineData(11, 54, 12, 6)]
        public void PastAndTwo_ModifiesMinutesWhenTo(int hour, int min, int expHour, int expMin) {
            b.Info.Flow();

            var st = new SmallTimeRenderer(new SmallTime(hour, min));
            var sut = new PastAndToCapability();

            sut.Convert(st);

            Assert.Equal(expHour, st.RenderTime.Hour);
            Assert.Equal(expMin, st.RenderTime.Minute);
        }
    }
}