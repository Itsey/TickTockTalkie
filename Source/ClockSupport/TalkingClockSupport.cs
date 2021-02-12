using Humanizer;
using Plisky.Diagnostics;
using System;

namespace Plisky.ClockChallenge {
    public class TalkingClockSupport {
        protected Bilge b = new Bilge("clock-support");

        private const int QUARTER_TO_MINS = 45;
        private const int QUARTER_PAST_MINS = 15;
        private const int HALF_PAST_MINS = 30;
        private const int MAXMINUTES_FOR_PASTHOUR = 5;
        private const int SPECIAL_ZERO_MIDNIGHT = 0;
        private const int SPECIAL_12_NOON = 12;

        public string ConvertTime(int hour, int minute) {
            var result = SpecialCase(hour, minute);
            if (result == null) {
                string minWords = minute.ToWords();
                string hrWords = hour.ToWords();

                if (minute < MAXMINUTES_FOR_PASTHOUR) {
                    result = $"{minWords} past {hrWords}";
                } else {
                    result = $"{hrWords} {minWords}";
                }
            }
            result = result.Trim();
            return result;
        }

        private string SpecialCase(int hour, int minute) {
            bool reverseHourMinuteTextOrder = false;
            string minuteText = null;
            string hourText = null;

            if (hour == SPECIAL_ZERO_MIDNIGHT) {
                hourText = "midnight";
                minuteText = minute.ToWords() + " past";
                reverseHourMinuteTextOrder = true;
            }
            if (hour == SPECIAL_12_NOON) {
                hourText = "noon";
                minuteText = minute.ToWords();
            }
            if (minute == 0) {
                minuteText = "";
            }

            if (minute == HALF_PAST_MINS) {
                minuteText = $"half past";
                hourText = hour.ToWords();
                reverseHourMinuteTextOrder = true;
            }

            if (minute == QUARTER_TO_MINS) {
                hourText ??= (++hour).ToWords();
                minuteText = "quarter to";
                reverseHourMinuteTextOrder = true;
            }

            if (minute == QUARTER_PAST_MINS) {
                hourText ??= hour.ToWords();
                minuteText = "quarter past";
                reverseHourMinuteTextOrder = true;
            }

            if (hourText != null) {
                b.Assert.NotNull(minuteText);

                return reverseHourMinuteTextOrder ? $"{minuteText} {hourText}" : $"{hourText} {minuteText}";
            }
            return null;
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
                    b.Verbose.Log("Special Case - appending minutes",timeToParse);                    
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
