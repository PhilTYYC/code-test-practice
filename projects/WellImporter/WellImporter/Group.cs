using System.Collections.Generic;
using WellImporter.Interfaces;

namespace WellImporter
{
    public class Group : IGroup
    {
        private List<IWell> _wells = new List<IWell>();

        public string Name { get; set; }

        public double X { get; set; }

        public double Y { get; set; }

        public LatLong Center => new LatLong() { X = X, Y = Y };

        public double Radius { get; set; }

        public IReadOnlyList<IWell> Children => _wells.AsReadOnly();

        public void AddChild(IWell well) => _wells.Add(well);

    }
}
