using BepInEx;
using UnityEngine;

namespace CupheadMod
{
    [BepInPlugin("com.test.cupheadmod", "CupheadMod", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static BepInEx.Logging.ManualLogSource Log;

        void Awake()
        {
            Log = Logger;
            Logger.LogInfo("Мод загружен!");
            GameObject go = new GameObject("CupheadMod");
            go.AddComponent<SceneMenu>();
            DontDestroyOnLoad(go);
        }
    }
}
