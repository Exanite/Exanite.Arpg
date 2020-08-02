using Newtonsoft.Json;
using UnityEngine;

namespace Exanite.Arpg.Modding
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ModInfo : ScriptableObject
    {
        public const string AssetsFolderName = "Assets";
        public const string BundlesFolderName = "Bundles";
        public const string SourceCodeFolderName = "Source";

        [SerializeField, HideInInspector] private string modId;
        [SerializeField, HideInInspector] private string modName;
        [SerializeField, HideInInspector] private string modDescription;
        [SerializeField, HideInInspector] private string modVersion;

        [JsonProperty]
        public string ModId
        {
            get
            {
                return modId;
            }

            set
            {
                modId = value;
            }
        }

        [JsonProperty]
        public string ModName
        {
            get
            {
                return modName;
            }

            set
            {
                modName = value;
            }
        }

        [JsonProperty]
        public string ModDescription
        {
            get
            {
                return modDescription;
            }

            set
            {
                modDescription = value;
            }
        }

        [JsonProperty]
        public string ModVersion
        {
            get
            {
                return modVersion;
            }

            set
            {
                modVersion = value;
            }
        }
    } 
}
