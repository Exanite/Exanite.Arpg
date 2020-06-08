namespace Exanite.Arpg.AssetManagement.Editor
{
    public abstract class PackageBuilder
    {
        public string packageName;
        public string buildDirectory;

        public abstract void Build();
    }
}