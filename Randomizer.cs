using BepInEx.Logging;
using Newtonsoft.Json;
using ReventureRando.ItemLocations;
using ReventureRando.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ReventureRando
{
    public class Randomizer
    {
        //Settings
        private List<ItemEnum> availableItems;
        private List<ItemLocationEnum> availableLocations;

        //State
        private Dictionary<ItemLocationEnum, ItemEnum> randomized;

        //GameObjects
        private Dictionary<ItemLocationEnum, ItemLocation> enumToLocation;
        private Dictionary<ItemEnum, Item> enumToItem;

        private ManualLogSource Logger;

        public Randomizer(ManualLogSource _l)
        {
            Logger = _l;
            Init();
        }

        public Randomizer(ManualLogSource _l, int seed)
        {
            Logger = _l;
            Random.InitState(seed);
            Init();
        }

        private void Init()
        {
            if (!LoadState())
            {
                Randomize();
                StoreState();
            }
        }

        public void Randomize()
        {
            LoadSettings();
            randomized = new Dictionary<ItemLocationEnum, ItemEnum>();
            List<ItemEnum> localCopyItems = new List<ItemEnum>(availableItems);
            List<ItemLocationEnum> localCopyLocations = new List<ItemLocationEnum>(availableLocations);

            while (localCopyLocations.Count > 0)
            {
                int randLocationIndex = Random.RandomRangeInt(0, localCopyLocations.Count);
                ItemLocationEnum loc = localCopyLocations[randLocationIndex];
                localCopyLocations.RemoveAt(randLocationIndex);

                ItemEnum item = ItemEnum.None;
                if (localCopyItems.Count() > 0)
                {
                    int randInd = Random.RandomRangeInt(0, localCopyItems.Count);
                    item = localCopyItems[randInd];
                    localCopyItems.RemoveAt(randInd);
                }
                randomized.Add(loc, item);
            }
        }

        public bool LoadState()
        {
            try
            {
                randomized = JsonConvert.DeserializeObject<Dictionary<ItemLocationEnum, ItemEnum>>(File.ReadAllText("randomizer.txt"));
                return true;
            } catch (Exception e)
            {
                Logger.LogError(e.Message);
                return false;
            }
        }

        public void StoreState()
        {
            string json = JsonConvert.SerializeObject(randomized);
            File.WriteAllText("randomizer.txt", json);
        }

        private void LoadSettings()
        {
            availableLocations = Enum.GetValues(typeof(ItemLocationEnum)).Cast<ItemLocationEnum>().ToList();
            availableItems = Enum.GetValues(typeof(ItemEnum)).Cast<ItemEnum>().Where(x => x != ItemEnum.None).ToList();
            //availableLocations = new List<ItemLocationEnum> { ItemLocationEnum.SwordPedestal, ItemLocationEnum.SwordAtHome };
            //availableItems = new List<ItemEnum> { ItemEnum.Sword, ItemEnum.Bomb };
        }

        public void Print()
        {
            foreach (ItemLocationEnum loc in randomized.Keys)
            {
                Logger.LogInfo($"{loc}: {randomized[loc]}");
            }
        }

        public void ApplyToWorld()
        {

            foreach (ItemLocationEnum locEnum in randomized.Keys)
            {
                ItemLocation loc = null;
                switch (locEnum)
                {
                    case ItemLocationEnum.SwordPedestal:
                        loc = new SwordPedestal();
                        break;
                    case ItemLocationEnum.SwordAtHome:
                        //Disable fake open chest if item in guard needs to be picked up
                        GameObject fakeOpenChest = GameObject.Find("World/Items/SwordAtHome/OpenChest");
                        if (fakeOpenChest != null)
                        {
                            fakeOpenChest.SetActive(false);
                        }
                        loc = new TreasureChestLocation("World/Items/SwordAtHome/TreasureChest_Sword");
                        break;
                    case ItemLocationEnum.TreasureRoom:
                        loc = new Treasureroom();
                        break;
                    case ItemLocationEnum.DeadGuard:
                        loc = new SwordInGuard();
                        break;
                    case ItemLocationEnum.TreasureChest_WhistleOfTime:
                        loc = new TreasureChestLocation("World/Items/TreasureChest_WhistleOfTime");
                        break;
                    case ItemLocationEnum.TreasureChest_Hook:
                        loc = new TreasureChestLocation("World/Items/TreasureChest_Hook");
                        break;
                    case ItemLocationEnum.TreasureChest_MrHugs:
                        loc = new TreasureChestLocation("World/Items/TreasureChest_MrHugs");
                        break;
                    case ItemLocationEnum.TreasureChest_Trinket:
                        loc = new TreasureChestLocation("World/Items/TreasureChest_Trinket");
                        break;
                    case ItemLocationEnum.TreasureChest_Shovel:
                        loc = new TreasureChestLocation("World/Items/TreasureChest_Shovel");
                        break;
                    case ItemLocationEnum.TreasureChest_Cannonball:
                        loc = new TreasureChestLocation("World/Items/TreasureChest_Cannonball");
                        break;
                    case ItemLocationEnum.TreasureChest_FishingRod:
                        loc = new TreasureChestLocation("World/Items/TreasureChest_FishingRod");
                        break;
                    case ItemLocationEnum.TreasureChest_Shield:
                        loc = new TreasureChestLocation("World/Items/TreasureChest_Shield");
                        break;
                    case ItemLocationEnum.TreasureChest_MyPhone:
                        loc = new TreasureChestLocation("World/Items/TreasureChest_MyPhone");
                        break;
                    case ItemLocationEnum.TreasureChest_Compass:
                        loc = new TreasureChestLocation("World/Items/TreasureChest_Compass");
                        break;
                    case ItemLocationEnum.TreasureChest_Map:
                        loc = new TreasureChestLocation("World/Items/TreasureChest_Map");
                        break;
                    case ItemLocationEnum.TreasureChest_Pizza:
                        loc = new TreasureChestLocation("World/Items/TreasureChest_Pizza");
                        break;
                    case ItemLocationEnum.TreasureChest_DarkStone:
                        loc = new TreasureChestLocation("World/Interactables/Levers/RightLeversPlatform/TreasureChest_DarkStone");
                        break;
                    case ItemLocationEnum.TreasureChest_Bomb:
                        loc = new TreasureChestLocation("World/Items/TreasureChest_Bomb");
                        break;
                    case ItemLocationEnum.AnvilRope:
                        loc = new AnvilRope();
                        break;
                    case ItemLocationEnum.Princess:
                        loc = new PrincessLocation();
                        break;
                    case ItemLocationEnum.OrbtaleLocation:
                        loc = new OrbtaleLocation();
                        break;
                    case ItemLocationEnum.Shopkeeper:
                        loc = new ShopkeeperLocation();
                        break;
                    case ItemLocationEnum.BoomerangLocation:
                        loc = new BoomerangLocation();
                        break;
                    case ItemLocationEnum.ChickenLocation:
                        loc = new ChickenLocation("World/PersistentElements/ChickenNest/Phase3", "Chicken");
                        break;
                    case ItemLocationEnum.DarkChickenLocation:
                        loc = new ChickenLocation("World/Items", "DarkChicken");
                        break;
                }
                ItemEnum itemEnum = randomized[locEnum];
                Item item = null;
                switch (itemEnum)
                {
                    case ItemEnum.None:
                        break;
                    case ItemEnum.Sword:
                        item = new ReventureItem(ItemTypes.Sword);
                        break;
                    case ItemEnum.Bomb:
                        item = new ReventureItem(ItemTypes.Bomb);
                        break;
                    case ItemEnum.Hook:
                        item = new ReventureItem(ItemTypes.Hook);
                        break;
                    case ItemEnum.WhistleOfTime:
                        item = new ReventureItem(ItemTypes.Whistle);
                        break;
                    case ItemEnum.MrHugs:
                        item = new ReventureItem(ItemTypes.MrHugs);
                        break;
                    case ItemEnum.Shovel:
                        item = new ReventureItem(ItemTypes.Shovel);
                        break;
                    case ItemEnum.Trinket:
                        item = new ReventureItem(ItemTypes.LavaTrinket);
                        break;
                    case ItemEnum.Nuke:
                        item = new ReventureItem(ItemTypes.Nuke);
                        break;
                    case ItemEnum.FishingRod:
                        item = new ReventureItem(ItemTypes.PhishingRod);
                        break;
                    case ItemEnum.Shield:
                        item = new ReventureItem(ItemTypes.Shield);
                        break;
                    case ItemEnum.MyPhone:
                        item = new ReventureItem(ItemTypes.MyPhone);
                        break;
                    case ItemEnum.Compass:
                        item = new ReventureItem(ItemTypes.Compass);
                        break;
                    case ItemEnum.Map:
                        item = new ReventureItem(ItemTypes.Map);
                        break;
                    case ItemEnum.Chicken:
                        item = new ReventureItem(ItemTypes.Chicken);
                        break;
                    case ItemEnum.Orbtale:
                        item = new ReventureItem(ItemTypes.Cardventure);
                        break;
                    case ItemEnum.Burger:
                        item = new ReventureItem(ItemTypes.Pizza);
                        break;
                    case ItemEnum.DarkStone:
                        item = new ReventureItem(ItemTypes.DarkStone);
                        break;
                    case ItemEnum.Shotgun:
                        item = new ReventureItem(ItemTypes.Shotgun);
                        break;
                    case ItemEnum.Anvil:
                        item = new ReventureItem(ItemTypes.Anvil);
                        //Disable anvil cutscene
                        TreasureItem anvilItem = GameObject.Find("World/Items/AnvilRope/SwingRope/Item Anvil").GetComponent<TreasureItem>();
                        anvilItem.onItemPicked.m_PersistentCalls.RemoveListener(1);
                        anvilItem.BeforePick.m_PersistentCalls.RemoveListener(0);
                        break;
                    case ItemEnum.Princess:
                        item = new ReventureItem(ItemTypes.Princess);
                        break;
                    case ItemEnum.Boomerang:
                        item = new ReventureItem(ItemTypes.Boomerang);
                        break;
                }
                loc.ReplaceItem(item);
            }
        }
    }
}
