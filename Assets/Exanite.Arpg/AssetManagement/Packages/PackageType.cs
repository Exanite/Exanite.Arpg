using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Exanite.Arpg.AssetManagement.Packages
{
    [Serializable]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PackageType
    {
        AssetBundle,
    }
}
