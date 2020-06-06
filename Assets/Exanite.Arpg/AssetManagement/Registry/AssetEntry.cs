using System;

namespace Exanite.Arpg.AssetRegistry
{
    public class AssetEntry
    {
        private string path;
        private string package;
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
