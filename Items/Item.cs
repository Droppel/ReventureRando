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

        public GameObject GetPrefab()
        {
            return prefab;
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
        Pizza,
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
