namespace Plisky.ClockChallenge {

    using Humanizer;

    // Stick multiple in same file here as they all broadly do the same thing.

    public class DefaultNumbersToWords : ConverterChainBase {

        protected override void Process(SmallTimeRenderer st) {
            string minWords = st.RenderTime.Minute.ToWords();
            string hrWords = st.RenderTime.Hour.ToWords();

            st.DefaultRenderText(RenderElements.HourText, hrWords);
            st.DefaultRenderText(RenderElements.Intermediary, " ");
            st.DefaultRenderText(RenderElements.MinuteText, minWords);
        }

        protected override void SetName() {
            ConverterName = nameof(DefaultNumbersToWords);
        }
    }

    public class MidnightCapability : ConverterChainBase {

        protected override void Process(SmallTimeRenderer st) {
            if ((st.RenderTime.Hour == SPECIAL_ZERO_MIDNIGHT) || (st.RenderTime.Hour == SPECIAL_TWENTYFOUR_MIDNIGHT)) {
                st.ForceRenderText(RenderElements.HourText, "midnight");
            }
            if (st.RenderTime.Hour == SPECIAL_12_NOON) {
                st.ForceRenderText(RenderElements.HourText, "noon");
            }
        }

        protected override void SetName() {
            ConverterName = nameof(MidnightCapability);
        }
    }

    public class QuarterHalfAndExact : ConverterChainBase {

        protected override void Process(SmallTimeRenderer st) {
            if ((st.RenderTime.Minute == QUARTER_PAST_MINS) || (st.RenderTime.Minute == QUARTER_TO_MINS)) {
                st.ForceRenderText(RenderElements.MinuteText, "quarter");
            }
            if (st.RenderTime.Minute == HALF_PAST_MINS) {
                st.ForceRenderText(RenderElements.MinuteText, "half");
            }
            if (st.RenderTime.Minute == 0) {
                st.ForceRenderText(RenderElements.Intermediary, " exactly ");
                st.FormatString = "%HUR%%INT%";  // Remove the minute capability
            }
        }

        protected override void SetName() {
            ConverterName = nameof(QuarterHalfAndExact);
        }
    }

    public class PastAndToCapability : ConverterChainBase {

        protected override void Process(SmallTimeRenderer st) {
            if (st.RenderTime.Minute <= HALF_PAST_MINS) {
                st.DefaultRenderText(RenderElements.Intermediary, " past ");
            } else {
                st.DefaultRenderText(RenderElements.Intermediary, " to ");
                st.RenderTime.Hour += 1;
                st.RenderTime.Minute = MINUTESINHOUR - st.Time.Minute;
            }
        }

        protected override void SetName() {
            ConverterName = nameof(PastAndToCapability);
        }
    }
}