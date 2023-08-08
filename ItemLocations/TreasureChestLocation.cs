using ReventureRando.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ReventureRando.ItemLocations
{
    public class TreasureChestLocation : ItemLocation
    {
        TreasureChest chest;

        public TreasureChestLocation(string name) : base(name)
        {
            chest = gameObject.GetComponent<TreasureChest>();
        }

        protected override GameObject DisableOldItem()
        {
            return null;
        }

        protected override void EnableNewItem(Item item, GameObject oldGameObject)
        {
            if (item == null)
            {
                chest.content = null;
                return;
            }
            chest.content = item.GetPrefab();
        }
    }
}
