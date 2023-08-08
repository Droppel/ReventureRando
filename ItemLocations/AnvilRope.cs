using ReventureRando.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ReventureRando.ItemLocations
{
    public class AnvilRope : ItemLocation
    {
        public AnvilRope() : base("World/Items/AnvilRope/SwingRope")
        {
            //TreasureItem anvilItem = GameObject.Find("World/Items/AnvilRope/SwingRope/Item Anvil").GetComponent<TreasureItem>();
            //anvilItem.onItemPicked.m_PersistentCalls.RemoveListener(1);
            //anvilItem.BeforePick.m_PersistentCalls.RemoveListener(0);
        }

        protected override GameObject DisableOldItem()
        {
            GameObject oldItem = GameObject.Find("World/Items/AnvilRope/SwingRope/Item Anvil");
            oldItem.SetActive(false);
            return oldItem;
        }

        protected override void EnableNewItem(Item item, GameObject oldGameObject)
        {
            SpawnNewItem(item, oldGameObject, "Anvil Rope Item");
        }
    }
}
