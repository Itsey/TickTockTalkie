namespace Plisky.ClockChallenge {

    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Small time rendering is just hours and minutes and some info on formatting, focuses on the rendering aspect of the problem.
    /// </summary>
    [DebuggerDisplay("{Hour}:{Minute} ({Render()})")]
    public class SmallTimeRenderer {
        public SmallTime Time;
        public SmallTime RenderTime;

        public Dictionary<RenderElements, string> RenderParts = new Dictionary<RenderElements, string>();

        public string FormatString { get; set; }

        public void DefaultRenderText(RenderElements re, string newValue) {
            if (string.IsNullOrEmpty(RenderParts[re])) {
                ForceRenderText(re, newValue);
            }
        }

        public void ForceRenderText(RenderElements re, string newValue) {
            if (newValue == null) {
                newValue = string.Empty;
            }
            RenderParts[re] = newValue;
        }

        public string GetRenderText(RenderElements re) {
            return RenderParts[re];
        }

        public string Render() {
            // This is allowing for full control of the returned string - probably overengineered!
            return FormatString.Replace("%PRE%", GetRenderText(RenderElements.Preamble))
                               .Replace("%HUR%", GetRenderText(RenderElements.HourText))
                               .Replace("%INT%", GetRenderText(RenderElements.Intermediary))
                               .Replace("%MIN%", GetRenderText(RenderElements.MinuteText))
                               .Replace("%PST%", GetRenderText(RenderElements.Postamble));
        }

        public SmallTimeRenderer(SmallTime st) {
            RenderParts.Add(RenderElements.Preamble, string.Empty);
            RenderParts.Add(RenderElements.MinuteText, string.Empty);
            RenderParts.Add(RenderElements.Intermediary, string.Empty);
            RenderParts.Add(RenderElements.Postamble, string.Empty);
            RenderParts.Add(RenderElements.HourText, string.Empty);
            Time = st;
            RenderTime.Hour = st.Hour;
            RenderTime.Minute = st.Minute;
            FormatString = "%PRE%%MIN%%INT%%HUR%%PST%";
        }
    }
}