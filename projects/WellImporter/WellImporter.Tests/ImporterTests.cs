using System.Linq;
using Xunit;
using System.Collections.Generic;

namespace WellImporter.Tests
{
    public class ImporterTests
    {
        [Fact]
        public void ImporterWellShouldCreateWell()
        {
            var wellsData = new[] { "Well, Well A, 1, 1, 1, 1" };

            var data = Importer.Import(wellsData);

            Assert.True(data.Wells.Count == 1);
            Assert.Equal( wellsData.Length, data.Wells.Count );
            Assert.NotNull(data.Wells[0]);
            AssertWell(data, "Well A", 1, 1, 1, 1, WellType.Vertical, null);
        }

        [Fact]
        public void ImporterInvalidWellShouldBeIgnored()
        {
            var wellsData = new[] { "Well, Well A, A, A, 1, 1" };

            var data = Importer.Import(wellsData);

            Assert.Equal(0, data.Wells.Count);
        }

        [Fact]
        public void ImporterWellsShouldCreateMultipleWells()
        {
            var wellsData = new[] 
            { 
                "Well, Well A, 1, 1, 1, 1", 
                "Well, Well B, 1, 20, 20, 1"
            };

            var data = Importer.Import(wellsData);

            Assert.Equal(data.Wells.Count, wellsData.Length);
            Assert.NotNull(data.Wells[0]);
            Assert.NotNull(data.Wells[1]);
            AssertWell(data, "Well A", 1, 1, 1, 1, WellType.Vertical, null);
            AssertWell(data, "Well B", 1, 20, 20, 1, WellType.Horizontal, null);
        }

        [Fact]
        public void ImporterWellsShouldCreateUniqueWells()
        {
            var wellsData = new[]
            {
                "Well, Well A, 1, 1, 1, 1",
                "Well, Well A, 1, 20, 20, 1"
            };

            var data = Importer.Import(wellsData);

            Assert.Equal(1, data.Wells.Count);
            Assert.NotNull(data.Wells[0]);
            AssertWell(data, "Well A", 1, 1, 1, 1, WellType.Vertical, null);
        }        

        [Fact]
        public void ImporterShouldCreateGroup()
        {
            var data = Importer.Import(new[] {
                "Group, Group A, 2, 2, 2"
            });

            Assert.Equal(1, data.Groups.Count);
            Assert.NotNull(data.Groups[0]);
            AssertGroup(data, "Group A", 2, 2, 2, new string[] {});
        }
        [Fact]
        public void ImporterImportingGroupShouldIgnoreInvalidInput()
        {
            var data = Importer.Import(new[] {
                "Group, Group A, A, A, 2"
            });

            Assert.Equal(0, data.Groups.Count);
        }

        [Fact]
        public void ImporterShouldCreateUniqueGroups()
        {
            var data = Importer.Import(new[] {
                "Group, Group A, 2, 2, 2",
                "Group, Group B, 16, 9, 3",
                "Group, Group A, 32, 10, 5"
            });

            Assert.Equal(2, data.Groups.Count);
            Assert.NotNull(data.Groups[0]);
            Assert.NotNull(data.Groups[1]);
            AssertGroup(data, "Group A", 2, 2, 2, new string[] { });
            AssertGroup(data, "Group B", 16, 9, 3, new string[] { });
        }

        [Fact]
        public void ImporterShouldImportWellAndGroup()
        {
            var data = Importer.Import(new [] {
                "Well, Well A, 1, 1, 1, 1",
                "Group, Group A, 2, 2, 2"
            });

            AssertWell(data, "Well A", 1, 1, 1, 1, WellType.Vertical, "Group A");
            AssertGroup(data, "Group A", 2, 2, 2, new [] { "Well A" });
            Assert.Equal(1, data.Wells.Count);
            Assert.Equal(1, data.Groups.Count);
        }

        [Fact]
        public void ImportWellsAndGroupsNoOverlap()
        {
            var data = Importer.Import(new [] {
                "Well, Well A, 5, 3, 5, 3",
                "Well, Well B, 8, 9, 9, 10",
                "Group, Group A, 6, 6, 5",
                "Well, Well C, 12, 4, 18, 2",
                "Well, Well D, 16, 7, 16, 7",
                "Group, Group B, 16, 9, 3",
                "Well, Well E, 15, 11, 14, 8",
                "Well, Well F, 3, 7, 3, 7"
            });

            AssertWell(data, "Well A", 5, 3, 5, 3, WellType.Vertical, "Group A");
            AssertWell(data, "Well B", 8, 9, 9, 10, WellType.Slanted, "Group A");
            AssertWell(data, "Well C", 12, 4, 18, 2, WellType.Horizontal, null);
            AssertWell(data, "Well D", 16, 7, 16, 7, WellType.Vertical, "Group B");
            AssertWell(data, "Well E", 15, 11, 14, 8, WellType.Slanted, "Group B");
            AssertWell(data, "Well F", 3, 7, 3, 7, WellType.Vertical, "Group A");
            AssertGroup(data, "Group A", 6, 6, 5, new [] { "Well A", "Well B", "Well F" });
            AssertGroup(data, "Group B", 16, 9, 3, new [] { "Well D", "Well E" });
            Assert.Equal(6, data.Wells.Count);
            Assert.Equal(2, data.Groups.Count);
        }

        [Fact]
        public void ImportWellAndGroupsWithOverlapAddToClosestGroupCenter()
        {
            var data = Importer.Import(new[] {
                "Well, Well A, 4, 4, 4, 4",
                "Group, Group A, 1, 4, 7",
                "Group, Group B, 5, 4, 7"
            });
            
            AssertWell(data, "Well A", 4, 4, 4, 4, WellType.Vertical, "Group B");
            AssertGroup(data, "Group A", 1, 4, 7, new string[0]);
            AssertGroup(data, "Group B", 5, 4, 7, new[] { "Well A" });
        }

        [Fact]
        public void ImportWellAndGroupsWithOverlapAddFirstAlphabeticalGroup()
        {
            var data = Importer.Import(new[] {
                "Well, Well A, 4, 4, 4, 4",
                "Group, Group B, 2, 4, 7",
                "Group, Group A, 6, 4, 7"
            });

            AssertWell(data, "Well A", 4, 4, 4, 4, WellType.Vertical, "Group A");
            AssertGroup(data, "Group B", 2, 4, 7, new string[0]);
            AssertGroup(data, "Group A", 6, 4, 7, new[] { "Well A" });
        }

        private void AssertWell(ImportedData data, string name, double topX, double topY, double bottomX, double bottomY, WellType wellType, string parent) {
            var well = data.Wells.First(w => w.Name == name);
            Assert.Equal(name, well.Name);
            Assert.Equal(topX, well.TopHoleX, 5);
            Assert.Equal(topY, well.TopHoleY, 5);
            Assert.Equal(bottomX, well.BottomHoleX, 5);
            Assert.Equal(bottomY, well.BottomHoleY, 5);
            Assert.Equal(wellType, well.WellType);

            if(parent != null) {
                var group = data.Groups.First(g => g.Name == parent);
                Assert.NotNull(group.Children.First(w => w.Name == well.Name));
            }
        }

        private void AssertGroup(ImportedData data, string name, double x, double y, double radius, string[] children) {
            var group = data.Groups.First(g => g.Name == name);
            Assert.Equal(name, group.Name);
            Assert.Equal(x, group.X, 5);
            Assert.Equal(y, group.Y, 5);
            Assert.Equal(radius, group.Radius);
            if(children == null || children.Length == 0)
                AssertNullOrEmpty(group.Children);
            else
                Assert.Equal(children, group.Children.Select(w => w.Name));
        }

        private void AssertNullOrEmpty(IReadOnlyList<object> list) {
            if(list == null)
                return;
            Assert.Equal(0, list.Count);
        }
    }
}
