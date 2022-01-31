using System.Collections.Generic;
using System.IO;
using ItemChanger;
using Newtonsoft.Json;

namespace RecentItemsDisplay
{
    public static class AreaName
    {
        private static readonly List<string> suffixes = new List<string>() { "_boss_defeated", "_boss", "_preload" };

        private static Dictionary<string, string> sceneToArea;

        public static void LoadData()
        {
            JsonSerializer js = new JsonSerializer
            {
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Auto,
            };

            Stream json = typeof(AreaName).Assembly.GetManifestResourceStream("RecentItemsDisplay.Resources.SceneToArea.json");
            StreamReader sr = new StreamReader(json);
            JsonTextReader jtr = new JsonTextReader(sr);
            sceneToArea = js.Deserialize<Dictionary<string, string>>(jtr);
        }

        public static string CleanAreaName(string scene)
        {
            if (string.IsNullOrEmpty(scene)) return string.Empty;

            foreach (string suffix in suffixes)
            {
                if (scene.EndsWith(suffix))
                {
                    scene = scene.Substring(0, scene.Length - suffix.Length);
                }
            }

            switch (scene)
            {
                // We can treat shops and scenes with just an NPC as special cases
                case SceneNames.Room_mapper: return "伊塞尔达";
                case SceneNames.Room_shop: return "斯莱";
                case SceneNames.Room_Charm_Shop: return "萨鲁巴";
                case SceneNames.Fungus2_26: return "食腿者";
                case SceneNames.Crossroads_38: return "虫爷爷";
                case SceneNames.RestingGrounds_07: return "先知";
                case SceneNames.Room_Ouiji: return "吉吉";
                case SceneNames.Room_Jinn: return "吉恩";
                case SceneNames.Grimm_Divine: return "迪万";
                // Forward compatibility
                case SceneNames.Room_nailsmith: return "钉子匠";
                case SceneNames.Room_Mask_Maker: return "面具制作师";
                case SceneNames.Room_Tram:
                case SceneNames.Room_Tram_RG:
                    return "电车";

                default:
                    if (sceneToArea.TryGetValue(scene, out string area))
                    {
                        return area;
                    }
                    else if (scene.StartsWith("GG_"))
                    {
                        // GG_Waterways, Pipeway, Shortcut and Lurker are in the JSON
                        return "Godhome";
                    }
                    else
                    {
                        return string.Empty;
                    }
            }
        }

    }
}
