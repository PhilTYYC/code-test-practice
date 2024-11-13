using System;
using Xunit;
using BadClock;

namespace BadClock.Tests
{
    public class Test
    {
        [Fact]
        public void SkewTimeIsAheadAndGainIsNegThree()
        {
            Assert.Equal(70.0, Clock.NextAgreement("11:59:58", "12:03:28", -3), 9);
        }

        [Fact]
        public void SkewTimeIsAheadAndGainIsNegOne()
        {
            Assert.Equal(1.0, Clock.NextAgreement("01:01:01", "01:01:02", -1), 9);
        }

        [Fact]
        public void SkewTimeIsOneSecondAheadAndGainIsNegThree()
        {
            Assert.Equal(0.3333333333, Clock.NextAgreement("01:01:01", "01:01:02", -3), 9);
        }

        [Fact]
        public void SkewTimeIsOneHourAheadAndTimeIntervalIsNegOneMinute()
        {
            Assert.Equal(60, Clock.NextAgreement("01:00:00", "02:00:00", -60), 9);
        }

        [Fact]
        public void SkewTimeIsOneHourAheadAndTimeIntervalIsOneMinute()
        {
            Assert.Equal(11, Clock.NextAgreement("01:00:00", "02:00:00", 3600), 9);
        }


        [Fact]
        public void SkewTimeIsOneSecondAheadAndHourlyGainIsThreeSeconds()
        {
            Assert.Equal(14399.666666667, Clock.NextAgreement("01:01:01", "01:01:02", 3), 9);
        }


        [Fact]
        public void SkewTimeIsOneHourBehindAndHourlyGainIsThreeSeconds()
        {
            Assert.Equal(1200, Clock.NextAgreement("02:00:00", "01:00:00", 3), 9);
        }

        [Fact]
        public void SkewTimeIsOneHourBehindAndHourlyGainIsNegThreeSeconds()
        {
            Assert.Equal(13200, Clock.NextAgreement("02:00:00", "01:00:00", -3), 9);
        }

        [Fact]
        public void SkewTimeIsOneHourBehindAndHourlyGainIsNegOneHour()
        {
            Assert.Equal(10, Clock.NextAgreement("03:00:00", "01:00:00", -3600), 9);
        }

        [Fact]
        public void HourlyGainCannotBeZero()
        {
            Assert.Throws<InvalidOperationException>(() => Clock.NextAgreement("11:59:58", "12:03:28", 0));
        }
    }
}
