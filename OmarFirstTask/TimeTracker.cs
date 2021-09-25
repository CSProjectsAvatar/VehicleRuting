using System;
using System.Diagnostics;

namespace OmarFirstTask
{
   public class TimeTracker{
        private Stopwatch globalCrono {get; set;} = new Stopwatch();
        private Stopwatch divingCrono {get; set;} = new Stopwatch();
        private long TotalTimeInMs { get; set; }
        private long DivingTimeInMs { get; set; }
        private long TotalAvailableTimeInMs => Math.Max(0, TotalTimeInMs - globalCrono.ElapsedMilliseconds);
        private long DivingAvailableTimeInMs => Math.Max(0, DivingTimeInMs - divingCrono.ElapsedMilliseconds);
        public bool ExaustedTotalTime => TotalAvailableTimeInMs <= 0;
        public bool ExaustedDivingTime => DivingAvailableTimeInMs <= 0;
        public TimeTracker(long totalTimeInSeconds, long divingTimeInSeconds){
            TotalTimeInMs = totalTimeInSeconds*1000;
            DivingTimeInMs = divingTimeInSeconds*1000;
        }
        public void RestartGlobalCrono(){
            globalCrono.Restart();
        }
        public void RestartDivingCrono(){
            divingCrono.Restart();
        }
    }
}
