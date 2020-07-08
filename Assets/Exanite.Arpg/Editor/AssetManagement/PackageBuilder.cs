using System;

namespace Exanite.Arpg.Editor.AssetManagement
{
    public abstract class PackageBuilder
    {
        private string packageName;
        private string buildDirectory;

        public string PackageName
        {
            get
            {
                return packageName;
            }

            set
            {
                packageName = value;
            }
        }

        public string BuildDirectory
        {
            get
            {
                return buildDirectory;
            }

            set
            {
                buildDirectory = value;
            }
        }

        public virtual void Build()
        {
            ValidateProperties();
        }

        protected virtual void ValidateProperties()
        {
            if (string.IsNullOrWhiteSpace(PackageName))
            {
                throw new ArgumentException(nameof(PackageName));
            }

            if (string.IsNullOrWhiteSpace(BuildDirectory))
            {
                throw new ArgumentException(nameof(BuildDirectory));
            }
        }
    }
}