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

        public static Dictionary<ItemTypes, ItemTypes> randomized;

        private void Awake()
        {
            // Plugin startup logic
            PatchLogger = Logger;
            //Random.InitState(42);

            harmony = new Harmony(PluginInfo.PLUGIN_GUID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Logger.LogInfo("Methods patched");
            var patched = harmony.GetPatchedMethods();
            foreach (MethodInfo p in patched)
            {
                Logger.LogInfo($"Patched {p.FullDescription()}");
            }

            if (!LoadSeed())
            {
                Logger.LogInfo($"Loading Seed failed, creating new one");
                CreateSeed();
            }
            PrintSeed(randomized);
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }

        private bool LoadSeed()
        {
            try
            {
                randomized = JsonConvert.DeserializeObject<Dictionary<ItemTypes, ItemTypes>>(File.ReadAllText("randomizer.txt"));
                return true;
            } catch (Exception e)
            {
                Logger.LogError(e.Message);
                return false;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                CreateSeed();
                Logger.LogInfo($"New Seed created");
                PrintSeed(randomized);
            }
        }

        private void CreateSeed()
        {
            randomized = RandomizeItems();
            string json = JsonConvert.SerializeObject(randomized);
            File.WriteAllText("randomizer.txt", json);
        }

        private Dictionary<ItemTypes, ItemTypes> RandomizeItems()
        {
            List<ItemTypes> items = new List<ItemTypes>() { ItemTypes.Sword, ItemTypes.Shovel, ItemTypes.MrHugs, ItemTypes.Hook, ItemTypes.Nuke, ItemTypes.Bomb,
                ItemTypes.Shield, ItemTypes.LavaTrinket, ItemTypes.Princess, ItemTypes.DarkStone, ItemTypes.Shotgun, ItemTypes.Chicken, ItemTypes.Whistle, ItemTypes.Pizza,
                ItemTypes.Compass, ItemTypes.Map};
            List<ItemTypes> itemspicker = new List<ItemTypes>(items);

            Logger.LogInfo($"Items: {items}");
            Dictionary<ItemTypes, ItemTypes>  rand = new Dictionary<ItemTypes, ItemTypes>();
            foreach (ItemTypes item in items)
            {
                int randIndex = Random.RandomRangeInt(0, itemspicker.Count);
                rand.Add(item, itemspicker[randIndex]);
                itemspicker.RemoveAt(randIndex);
            }
            return rand;
        }

        private void PrintSeed(Dictionary<ItemTypes, ItemTypes> rand)
        {
            foreach (ItemTypes item in rand.Keys)
            {
                Logger.LogInfo($"{item}: {rand[item]}");
            }
        }

        private void OnDestroy()
        {
            harmony.UnpatchSelf();
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} unloaded!");
        }
    }

    [HarmonyPatch(typeof(Hero))]
    public class HeroPatch
    {
        [HarmonyPatch("ActivateSkill")]
        private static bool Prefix(ref Hero __instance, ref CharacterItem itemPrefab)
        {
            if (!Plugin.randomized.ContainsKey(itemPrefab.ItemType))
            {
                return true;
            }

            ItemTypes newItem = Plugin.randomized[itemPrefab.ItemType];
            Plugin.PatchLogger.LogInfo($"Replacing {itemPrefab.ItemType} with {newItem}");
            switch (newItem)
            {
                case ItemTypes.Sword:
                case ItemTypes.Shovel:
                case ItemTypes.MrHugs:
                case ItemTypes.Hook:
                case ItemTypes.Bomb:
                case ItemTypes.Shield:
                case ItemTypes.DarkStone:
                case ItemTypes.Map:
                case ItemTypes.Compass:
                case ItemTypes.Pizza:
                //case ItemTypes.MyPhone:
                    itemPrefab = GameObject.Find($"TreasureChest_{newItem}").GetComponent<TreasureChest>().content.GetComponent<TreasureItem>().ItemGrantedPrefab;
                    Plugin.PatchLogger.LogInfo($"Normal Chest: {newItem}");
                    return true;
                case ItemTypes.Whistle:
                    itemPrefab = GameObject.Find($"TreasureChest_WhistleOfTime").GetComponent<TreasureChest>().content.GetComponent<TreasureItem>().ItemGrantedPrefab;
                    Plugin.PatchLogger.LogInfo($"Special Chest: {newItem}");
                    return true;
                case ItemTypes.Nuke:
                    itemPrefab = GameObject.Find($"TreasureChest_Cannonball").GetComponent<TreasureChest>().content.GetComponent<TreasureItem>().ItemGrantedPrefab;
                    Plugin.PatchLogger.LogInfo($"Special Chest: {newItem}");
                    return true;
                case ItemTypes.LavaTrinket:
                    itemPrefab = GameObject.Find($"TreasureChest_Trinket").GetComponent<TreasureChest>().content.GetComponent<TreasureItem>().ItemGrantedPrefab;
                    Plugin.PatchLogger.LogInfo($"Special Chest: {newItem}");
                    return true;
                //case ItemTypes.Anvil:
                case ItemTypes.Chicken:
                case ItemTypes.Shotgun:
                    itemPrefab = GameObject.Find($"Item {newItem}").GetComponent<TreasureItem>().ItemGrantedPrefab;
                    Plugin.PatchLogger.LogInfo($"Normal Item: {newItem}");
                    return true;
                case ItemTypes.Princess:
                    itemPrefab = GameObject.Find($"World/NPCs/Item Princess").GetComponent<TreasureItem>().ItemGrantedPrefab;
                    Plugin.PatchLogger.LogInfo($"Princess");
                    return true;
                //case ItemTypes.Strawberry:
                //    itemPrefab = GameObject.Find($"Strawberry").GetComponent<CharacterItem>();
                //    Plugin.PatchLogger.LogInfo($"Strawberry");
                //    return true;
                default:
                    return true;
            }
        }
    }

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
}
