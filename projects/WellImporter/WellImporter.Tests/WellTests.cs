using WellImporter.Interfaces;
using Xunit;

namespace WellImporter.Tests
{
    public class WellTests
    {
        [Fact]
        public void ImportedWellShouldBeVertical()
        {
            Well well = new Well();
            well.Name = "Well A";

            well.SetWellBore(1, 1, 1, 1);
            Assert.Equal(WellType.Vertical, well.WellType);

            well.SetWellBore(1, 0.6, 0.6, 1);
            Assert.Equal(WellType.Vertical, well.WellType);
        }


        [Fact]
        public void ImportedWellShouldBeSlanted()
        {
            Well well = new Well();
            well.Name = "Well A";

            well.SetWellBore(1, 2, 2, 1);
            Assert.Equal(WellType.Slanted, well.WellType);

            well.SetWellBore(1, 4, 4, 1);
            Assert.Equal(WellType.Slanted, well.WellType);
        }


        [Fact]
        public void ImportedWellShouldBeHorizontal()
        {
            Well well = new Well();
            well.Name = "Well A";

            well.SetWellBore(12, 4, 18, 2);
            Assert.Equal(WellType.Horizontal, well.WellType);

            well.SetWellBore(1, 15, 15, 1);
            Assert.Equal(WellType.Horizontal, well.WellType);
        }
    }
}
