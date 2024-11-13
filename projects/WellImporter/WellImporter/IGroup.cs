using System.Collections.Generic;
using WellImporter.Interfaces;

namespace WellImporter
{
    public interface IGroup
    {
         string Name { get; }
         double X { get; }
         double Y { get; }
         LatLong Center { get; }
         double Radius { get; }
         IReadOnlyList<IWell> Children { get; }
         void AddChild(IWell well);
    }
}