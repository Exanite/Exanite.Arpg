using System;

namespace Exanite.Arpg.AssetRegistry
{
    public class AssetEntry
    {
        private const string DefaultPackage = "Default";

        private string path;
        private string package = DefaultPackage;
        private Type type;

        public string Path
        {
            get
            {
                return path;
            }

            set
            {
                path = value;
            }
        }

        public string Package
        {
            get
            {
                return package;
            }

            set
            {
                package = value;
            }
        }

        public Type Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }
    }
}
