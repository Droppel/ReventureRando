using ReventureRando.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ReventureRando.ItemLocations
{
    public class Treasureroom : ItemLocation
    {
        public Treasureroom() : base("World/PersistentElements/TreasureLonk")
        {
            GameObject.Find("World/PersistentElements/TreasureMinionGuards").SetActive(true);
        }

        protected override GameObject DisableOldItem()
        {
            GameObject itemSword = GameObject.Find("World/PersistentElements/TreasureLonk/Item Sword");
            itemSword.SetActive(false);
            return itemSword;
        }

        protected override void EnableNewItem(Item item, GameObject oldGameObject)
        {
            SpawnNewItem(item, oldGameObject, "Treasure Room Item");
        }
    }
}
