using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Exanite.Modding
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ModInfo : ScriptableObject
    {
        public const string AssetsFolderName = "Assets";
        public const string BundlesFolderName = "Bundles";
        public const string SourceCodeFolderName = "Source";

        [SerializeField, HideInInspector] private string modID;
        [SerializeField, HideInInspector] private string modName;
        [SerializeField, HideInInspector] private string modDescription;
        [SerializeField, HideInInspector] private string modVersion;

        [JsonProperty]
        [ShowInInspector]
        public string ModID
        {
            get
            {
                return modID;
            }

            set
            {
                modID = value;
            }
        }

        [JsonProperty]
        [ShowInInspector]
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
        [ShowInInspector]
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
        [ShowInInspector]
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
