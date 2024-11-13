using System.Collections.Generic;
using WellImporter.Interfaces;

namespace WellImporter
{
    public class ImportedData
    {
        private Dictionary<string, IWell> _wells = new Dictionary<string, IWell>();
        private Dictionary<string, IGroup> _groups = new Dictionary<string, IGroup>();

        public IReadOnlyList<IWell> Wells => new List<IWell>(_wells.Values).AsReadOnly();

        public IReadOnlyList<IGroup> Groups => new List<IGroup>(_groups.Values).AsReadOnly();

        public void AddWell(IWell well) {
            _wells.Add(well.Name, well);
        }

        public void AddGroup(IGroup group) {
            _groups.Add(group.Name, group);
        }
               
        public void AddWellsToGroups()
        {
            foreach (var well in _wells.Values)
            {
                var groupCandidates = new List< (IGroup, double)>();
                foreach (var group in _groups.Values)
                {
                    var wellDistanceFromGroupCenter = Helpers.GetDistance(group.X, group.Y, well.TopHoleX, well.TopHoleY);
                    if (wellDistanceFromGroupCenter < group.Radius)
                    {
                        groupCandidates.Add((group, wellDistanceFromGroupCenter));
                    }
                }

                (IGroup, double) selected = (null,0);
                foreach( var candidate in groupCandidates )
                {
                    if( selected.Item1 == null )
                    {
                        selected = candidate;
                        continue;
                    }

                    // find closest
                    if(candidate.Item2 < selected.Item2 )
                    {
                        selected = candidate;
                    }
                    // handle cases where they are same distance, find the first alphabetically
                    else if(candidate.Item2 == selected.Item2)
                    {
                        int comp = string.Compare(candidate.Item1.Name, selected.Item1.Name);
                        if( comp == -1 )
                        {
                            selected = candidate;
                        }
                    }
                }
                if (selected.Item1 != null)
                {
                    selected.Item1.AddChild(well);
                }
            }
        }
    }
}