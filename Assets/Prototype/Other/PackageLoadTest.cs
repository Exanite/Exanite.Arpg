using Exanite.Arpg.AssetManagement.Packages;
using UnityEngine;

namespace Prototype
{
    public class PackageLoadTest : MonoBehaviour
    {
        public Package package;

        private void Start()
        {
            package = Package.LoadPackage(@"D:\Repositories\Exanite.Arpg\Assets\Mods\ExampleMod\Bundles\examplemod.packageinfo");

            Debug.Log(package.AssetBundle.name);

            foreach (var item in package.Entries)
            {
                Debug.Log(item.Key);
            }

            var obj = package.AssetBundle.LoadAsset<Sprite>("ExampleMod/Sprites/Flasks/Health/Flask_1");

            Debug.Log(obj);
        }
    }
}
