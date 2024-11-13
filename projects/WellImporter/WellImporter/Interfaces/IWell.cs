namespace WellImporter.Interfaces
{
    public interface IWell
    {
        string Name { get; }
        double TopHoleX { get; }
        double TopHoleY { get; }
        LatLong TopHole { get; }
        double BottomHoleX { get; }
        double BottomHoleY { get; }
        LatLong BottomHole { get; }
        WellType WellType { get; }
    }
}