using System;
using WellImporter.Interfaces;

namespace WellImporter
{
    public class Well : IWell
    {
        public string Name { get; set; }

        public double TopHoleX { get; private set; }

        public double TopHoleY { get; private set; }

        public LatLong TopHole => new LatLong() { X = TopHoleX, Y = TopHoleY };

        public double BottomHoleX { get; private set; }

        public double BottomHoleY { get; private set; }

        public LatLong BottomHole => new LatLong() { X = BottomHoleX, Y = BottomHoleY };

        public WellType WellType { get; private set; }

        public void SetWellBore( double topHoleX, double topHoleY, double bottomHoleX, double bottomHoleY )
        {
            TopHoleX = topHoleX;
            TopHoleY = topHoleY;
            BottomHoleX = bottomHoleX;
            BottomHoleY = bottomHoleY;

            var distance = Helpers.GetDistance(topHoleX, topHoleY, bottomHoleX, bottomHoleY);

            if( distance < 1.0 )
            {
                WellType = WellType.Vertical;
            }
            else if( distance >= 1 && distance < 5 )
            {
                WellType = WellType.Slanted;
            }
            else if( distance >= 5 )
            {
                WellType = WellType.Horizontal;
            }

        }
    }
}
