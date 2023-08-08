using ReventureRando.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ReventureRando.ItemLocations
{
    public class SwordPedestal : ItemLocation
    {
        public SwordPedestal() : base("World/Items/Sword Item Pedestal")
        {

        }

        protected override GameObject DisableOldItem()
        {
            GameObject itemSword = GameObject.Find("World/Items/Sword Item Pedestal/Item Sword");
            itemSword.SetActive(false);
            return itemSword;
        }

        protected override void EnableNewItem(Item item, GameObject oldGameObject)
        {
            SpawnNewItem(item, oldGameObject, "Item on Pedestal");
        }
    }
}
