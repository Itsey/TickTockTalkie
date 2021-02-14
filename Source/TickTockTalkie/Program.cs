using System;
using Plisky.Diagnostics;
using Plisky.Diagnostics.Listeners;

namespace Plisky.ClockChallenge {

    internal class Program {

        private static void Main(string[] args) {
#if DEBUG
            Bilge.AddHandler(new TCPHandler("127.0.0.1", 9060));
            Bilge.SetConfigurationResolver((nm, inval) => {
                return System.Diagnostics.SourceLevels.Verbose;
            });
#endif
            var b = new Bilge();
            Bilge.Alert.Online("ticktocktalkie");

            try {
                string timeStr = DateTime.Now.Hour + ":" + DateTime.Now.Minute;
                if (args.Length != 0) {
                    b.Verbose.Log("Time override from command line", args[0]);
                    timeStr = args[0];
                }

                string response;
                try {
                    IConvertTime ict = ConverterChainBase.GetFullChain();
                    var tsc = new TalkingClockSupport(ict);
                    var st = tsc.ParseTime(timeStr);
                    response = tsc.ConvertTime(new SmallTimeRenderer(st));
                } catch (ArgumentOutOfRangeException) {
                    response = "Invalid time format, please try HH:MM";
                }

                b.Verbose.Log($"Response [{response}]");
                Console.WriteLine(response);
            } catch (Exception ex) {
                b.Error.Dump(ex, "Unknown Fault");
                throw;
            }
        }
    }
}