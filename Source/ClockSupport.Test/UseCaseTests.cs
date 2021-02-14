namespace Plisky.ClockChallenge.Test {

    using Plisky.Diagnostics;
    using Plisky.Diagnostics.Listeners;
    using Plisky.Test;
    using Xunit;

    /// <summary>
    /// Complete use cases - the end result of what we have been working toward.  This replicates the full functionality of the
    /// program.  Integration style tests.
    /// </summary>
    public class UseCaseTests {
        private Bilge b = new Bilge(tl: System.Diagnostics.SourceLevels.Verbose);

        public UseCaseTests() {
            Bilge.AddHandler(new TCPHandler("127.0.0.1", 9060), HandlerAddOptions.SingleType);
            Bilge.SetConfigurationResolver((nm, lvl) => {
                return System.Diagnostics.SourceLevels.Verbose;
            });
        }

        [Theory(DisplayName = nameof(FullUseCasesWork))]
        [Trait(Traits.Age, Traits.Integration)]
        [Trait(Traits.Style, Traits.Unit)]
        [InlineData(1, 30, "half past one")]
        [InlineData(1, 29, "twenty-nine past one")]
        [InlineData(0, 15, "quarter past midnight")]
        [InlineData(1, 01, "one past one")]
        [InlineData(1, 3, "three past one")]
        [InlineData(1, 31, "twenty-nine to two")]
        [InlineData(0, 12, "twelve past midnight")]
        [InlineData(1, 45, "quarter to two")]
        [InlineData(1, 15, "quarter past one")]
        [InlineData(0, 0, "midnight")]
        [InlineData(12, 0, "noon")]
        public void FullUseCasesWork(int hour, int minute, string expected) {
            b.Info.Flow($"{hour}:{minute} -> {expected}");

            var sut = new TalkingClockSupport(ConverterChainBase.GetFullChain());
            var st = new SmallTimeRenderer(new SmallTime(hour, minute));
            string reply = sut.ConvertTime(st);

            b.Verbose.Log($"E[{expected}] A[{reply}]");
            Assert.Contains(expected, reply);
        }
    }
}