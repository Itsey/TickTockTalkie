namespace Plisky.ClockChallenge {
    
    using System;
    using Plisky.Diagnostics;

    public class TalkingClockSupport {
        protected Bilge b = new Bilge("clock-support");
        protected IConvertTime converter;

        public TalkingClockSupport(IConvertTime strategy) {
            converter = strategy;
        }

        public string ConvertTime(SmallTimeRenderer st) {
            b.Info.Flow();
            return converter.Convert(st);
        }

        public SmallTime ParseTime(string timeToParse) {
            b.Info.Flow($"{nameof(timeToParse)} {timeToParse ?? "null"}");
            if (string.IsNullOrEmpty(timeToParse)) {
                throw new ArgumentOutOfRangeException(nameof(timeToParse), "Unable to Parse null or empty strings to words.");
            }

            var result = new SmallTime();

            try {
                if (!timeToParse.Contains(":")) {
                    // Special case pass in 0 get 0:00 therefore midnight.
                    timeToParse += ":00";
                    b.Verbose.Log("Special Case - appending minutes", timeToParse);
                }

                result.Hour = DateTime.Parse(timeToParse).Hour;
                result.Minute = DateTime.Parse(timeToParse).Minute;

                return result;
            } catch (FormatException fx) {
                b.Error.Dump(fx, $"Exception parsing {timeToParse ?? "null"}");
                throw new ArgumentOutOfRangeException(nameof(timeToParse), fx);
            }
        }
    }
}