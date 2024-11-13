using System;

namespace WellImporter.Exceptions
{
    public class InvalidTokenInformationException : Exception
    {
        public InvalidTokenInformationException( string importType )
        {
            ImportType = importType;
        }

        public string ImportType { get; }
    }
}
