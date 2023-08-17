using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ReventureRando.Items
{
    public class ReventureItem : Item
    {
        public ReventureItem(ItemTypes reventureItemType, ItemEnum itemType) : base(itemType)
        {
            prefab = GetGameObjectFromReventureItemType(reventureItemType);
        }

        private GameObject GetGameObjectFromReventureItemType(ItemTypes itemType)
        {
            var allTreasureItems = Resources.FindObjectsOfTypeAll(typeof(TreasureItem)).Cast<TreasureItem>();
            foreach (TreasureItem tItem in allTreasureItems)
            {
                if (tItem.skill == itemType && tItem.ItemGrantedPrefab != null)
                {
                    return tItem.gameObject;
                }
            }
            return null;
        }
    }
}
