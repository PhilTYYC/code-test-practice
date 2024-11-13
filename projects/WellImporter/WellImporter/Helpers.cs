using System;

namespace WellImporter
{
    public static class Helpers
    {
        public static double GetDistance( double x1, double y1, double x2, double y2 )
        {
            var aSquared = Math.Pow(x1 - x2, 2);
            var bSquared = Math.Pow(y1 - y2, 2);
            var distance = Math.Sqrt(aSquared + bSquared);
            return distance;
        }
    }
}
