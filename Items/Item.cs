using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ReventureRando.Items
{
    [Serializable]
    public abstract class Item
    {
        protected GameObject prefab;
        protected ItemEnum itemType;

        public Item(ItemEnum _itemType)
        {
            itemType = _itemType;
        }

        public GameObject GetPrefab()
        {
            return prefab;
        }

        public ItemEnum GetItemType()
        {
            return itemType;
        }
    }
    public enum ItemEnum
    {
        None,
        Sword,
        Bomb,
        WhistleOfTime,
        Hook,
        MrHugs,
        Shovel,
        Trinket,
        Nuke,
        FishingRod,
        Shield,
        MyPhone,
        Compass,
        Map,
        DarkStone,
        Chicken,
        Burger,
        Shotgun,
        Orbtale,
        Anvil,
        Princess,
        Boomerang
    }
}
