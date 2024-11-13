using System;
using WellImporter.Exceptions;
using WellImporter.Interfaces;

namespace WellImporter
{
    public class Importer
    {
        public static ImportedData Import(string[] fileContents) {
            var data = new ImportedData();
            foreach (var line in fileContents)
            {
                var tokens = Tokenize(line);
                try
                {
                    switch (tokens[0])
                    {
                        case "Well":
                            data.AddWell(CreateWellFromTokens(tokens));
                            break;
                        case "Group":
                            data.AddGroup(CreateGroupFromTokens(tokens));
                            break;
                        default:
                            Console.WriteLine("Invalid Import Type");
                            break;
                    }
                }
                catch (InvalidTokenInformationException itie)
                {
                    Console.WriteLine(string.Format("Unable to {0}:{1}", itie.ImportType, itie.Message));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            data.AddWellsToGroups();
            return data;
        }

        private static string[] Tokenize(string line) {
            // Assume all the logic associated with parsing a CSV file is implement here.
            // Don't worry about edge cases like commas in names.
            return line.Split(",");
        }

        private static IWell CreateWellFromTokens(string[] tokens)
        {
            if (tokens.Length == 0 || tokens.Length > 6)
            {
                throw new InvalidTokenInformationException("Well");
            }

            Well well = new Well() { Name = tokens[1].Trim() };            
            var topHoleX = double.Parse(tokens[2]);
            var topHoleY = double.Parse(tokens[3]);
            var bottomHoleX = double.Parse(tokens[4]);
            var bottomHoleY = double.Parse(tokens[5]);
            well.SetWellBore(topHoleX, topHoleY, bottomHoleX, bottomHoleY);

            return well;
        }

        private static IGroup CreateGroupFromTokens(string[] tokens)
        {
            if (tokens.Length == 0 || tokens.Length > 5 )
            {
                throw new InvalidTokenInformationException("Group");
            }

            var group = new Group
            {
                Name = tokens[1].Trim(),
                X = double.Parse(tokens[2]),
                Y = double.Parse(tokens[3]),
                Radius = double.Parse(tokens[4])
            };

            return group;
        }
    }
}