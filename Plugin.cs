using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ReventureRando
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        Harmony harmony;
        public static ManualLogSource PatchLogger;

        public static Randomizer randomizer;

        private void Awake()
        {
            // Plugin startup logic
            PatchLogger = Logger;
            //Random.InitState(42);
            randomizer = new Randomizer(Logger);

            harmony = new Harmony(PluginInfo.PLUGIN_GUID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Logger.LogInfo("Methods patched");
            var patched = harmony.GetPatchedMethods();
            foreach (MethodInfo p in patched)
            {
                Logger.LogInfo($"Patched {p.FullDescription()}");
            }

            randomizer.Print();
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                randomizer.Randomize();
                randomizer.StoreState();
                Logger.LogInfo($"New Seed created");
                randomizer.Print();
            }
        }

        private void OnDestroy()
        {
            harmony.UnpatchSelf();
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} unloaded!");
        }

    }

    //[HarmonyPatch(typeof(Hero))]
    //public class HeroPatch
    //{
    //    [HarmonyPatch("ActivateSkill")]
    //    private static bool Prefix(ref Hero __instance, ref CharacterItem itemPrefab)
    //    {
    //        if (!Plugin.randomized.ContainsKey(itemPrefab.ItemType))
    //        {
    //            return true;
    //        }

    //        //ItemTypes newItem = Plugin.randomized[itemPrefab.ItemType];
    //        //Plugin.PatchLogger.LogInfo($"Replacing {itemPrefab.ItemType} with {newItem}");
    //        //itemPrefab = (CharacterItem)Resources.FindObjectsOfTypeAll(typeof(CharacterItem)).FirstOrDefault(g => ((CharacterItem)g).ItemType == newItem);
    //        return true;
    //    }
    //}

    // Allow skipping endings
    [HarmonyPatch(typeof(EndingProvider))]
    public class EndingProviderPatch
    {
        [HarmonyPatch("FinalizeRun", new Type[] { typeof(float), typeof(EndingCinematicConfiguration), typeof(bool) })]
        private static bool Prefix(ref EndingCinematicConfiguration configuration)
        {
            configuration.skippable = true;
            return true;
        }
    }

    // Allow skipping of initial Text
    [HarmonyPatch(typeof(SessionProvider))]
    public class SessionProviderPatch
    {
        [HarmonyPatch("IsInitialTextReaded", new Type[] { typeof(EndingTypes) })]
        private static bool Prefix(ref bool __result)
        {
            __result = true;
            return false;
        }
    }

    // Prevent Softlock for Empty Treasure Chest
    [HarmonyPatch(typeof(TreasureChest))]
    public class TreasureChestPatch
    {
        [HarmonyPatch("RetrieveItem", new Type[] {})]
        private static bool Prefix(ref TreasureChest __instance)
        {
            if (__instance.content == null)
            {
                __instance.Open();
                return false;
            }
            return true;
        }
    }

    
    [HarmonyPatch(typeof(GameplayDirector))]
    public class GameplayDirectorPatch
    {
        [HarmonyPatch("Start", new Type[] {})]
        private static void Postfix()
        {
            ////Enable Boomerang Pickup
            //if (Plugin.randomized.ContainsKey(ItemTypes.Boomerang))
            //{
            //    GameObject boomerangItem = GameObject.Find("World/Items/Item Boomerang");
            //    Plugin.PatchLogger.LogInfo(boomerangItem);
            //    boomerangItem.SetActive(true);
            //}
            Plugin.randomizer.ApplyToWorld();
            return;
        }
    }
}
