namespace Plisky.ClockChallenge {
    using System;
    using Plisky.Diagnostics;

    /// <summary>
    /// The converter chain holds a chain of capabilities that morph the time depending on the characteristics required.  There are two static helpers
    /// to build the chain - either one element at a time or the full chain.
    /// </summary>
    public abstract class ConverterChainBase : IConvertTime {

        #region statics and helpers

        public static ConverterChainBase GetFullChain() {
            return GetChain(new PastAndToCapability(), new QuarterHalfAndExact(), new MidnightCapability(), new DefaultNumbersToWords());
        }

        public static ConverterChainBase GetChain(params ConverterChainBase[] ccb) {
            if (ccb.Length == 0) {
                return null;
            }

            var result = ccb[0];
            var end = result;

            for (int i = 0; i < ccb.Length; i++) {
                end.Link(ccb[i]);
                end = ccb[i];
            }
            return result;
        }

        #endregion statics and helpers

        #region magic numbers

        protected const int QUARTER_TO_MINS = 45;
        protected const int QUARTER_PAST_MINS = 15;
        protected const int HALF_PAST_MINS = 30;
        protected const int MAXMINUTES_FOR_PASTHOUR = 5;
        protected const int MINUTESINHOUR = 60;
        protected const int SPECIAL_ZERO_MIDNIGHT = 0;
        protected const int SPECIAL_TWENTYFOUR_MIDNIGHT = 24;
        protected const int SPECIAL_12_NOON = 12;

        #endregion magic numbers

        protected abstract void Process(SmallTimeRenderer st);

        protected abstract void SetName();

        public string ConverterName { get; set; }

        protected Bilge b = new Bilge("converter-base");
        protected ConverterChainBase next = null;

        /// <summary>
        /// Add a new capability to the chain.
        /// </summary>
        /// <param name="nextLink">Capability to add</param>
        public virtual void Link(ConverterChainBase nextLink) {
            if (nextLink == null) {
                throw new InvalidOperationException("Do not link null");
            }
            next = nextLink;
        }

        public virtual SmallTimeRenderer Handle(SmallTimeRenderer payload) {
            b.Verbose.Log($"{ConverterName} Processing.");
            Process(payload);
            if (next != null) {
                return next.Handle(payload);
            }
            return payload;
        }

        /// <summary>
        /// Take the time, run it through the chain and return the rendered result.
        /// </summary>
        /// <param name="time">Time to render</param>
        /// <returns>Rendered string</returns>
        public string Convert(SmallTimeRenderer time) {
            return Handle(time).Render();
        }

        public ConverterChainBase() {
            SetName();
        }
    }
}