using System;
using Exanite.Arpg.AssetManagement.Packages;

namespace Exanite.Arpg.AssetRegistry
{
    public class AssetEntry
    {
        private Key key;
        private Package package;
        private Type type;

        public Key Key
        {
            get
            {
                return key;
            }

            set
            {
                key = value;
            }
        }

        public Package Package
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
