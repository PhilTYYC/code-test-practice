using System;
using System.Globalization;

namespace BadClock
{
    public class Clock
    {
        private const int SecondsPerMinute = 60;
        private const int MinutesPerHour = 60;

        public static double NextAgreement(string trueTime, string skewTime, int hourlyGain) {

            if( hourlyGain == 0 )
            {
                throw new InvalidOperationException("Hourly gGain cannot be zero.");
            }
            
            var trueTimeSecs = GetTimeInSeconds(trueTime);
            var skewTimeSecs = GetTimeInSeconds(skewTime);

            // Hourly gain determines which way we need to calculate the time difference
            var timeDifference = hourlyGain < 0 ? skewTimeSecs - trueTimeSecs : trueTimeSecs - skewTimeSecs;

            // if the difference is negative, we need to add a full 12 hours in seconds
            if( timeDifference < 0 )
            {
                timeDifference += GetMaxClockTimeInSeconds;
            }
            var nextAgreement = timeDifference / hourlyGain;
            return Math.Round( Math.Abs(nextAgreement), 9);
        }   
        
        public static double GetTimeInSeconds( string dateTimeString )
        {
            var timeParams = dateTimeString.Split(':');
            var hours = double.Parse(timeParams[0]);
            var minuites = double.Parse(timeParams[1]);
            var seconds = double.Parse(timeParams[2]);

            var hoursInSeconds = hours * MinutesPerHour * SecondsPerMinute;
            var minutesInSeconds = minuites * SecondsPerMinute;
            var totalSeconds = hoursInSeconds + minutesInSeconds + seconds;
            return totalSeconds;
        }

        public static double GetMaxClockTimeInSeconds => 12 * MinutesPerHour * SecondsPerMinute;

    }
}