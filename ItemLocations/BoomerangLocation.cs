using ReventureRando.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ReventureRando.ItemLocations
{
    public class BoomerangLocation : ItemLocation
    {
        public BoomerangLocation() : base("World/Items")
        {

        }

        protected override GameObject DisableOldItem()
        {
            GameObject oldItem = GameObject.Find("World/Items/Item Boomerang");
            oldItem.SetActive(false);
            return oldItem;
        }

        protected override void EnableNewItem(Item item, GameObject oldGameObject)
        {
            GameObject tChest = GameObject.Find("World/Items/TreasureChest_Bomb");
            GameObject newTreasureChest = GameObject.Instantiate(tChest, oldGameObject.transform.position, oldGameObject.transform.rotation, gameObject.transform);
            if (item == null)
            {
                newTreasureChest.GetComponent<TreasureChest>().content = null;
            } else
            {
                newTreasureChest.GetComponent<TreasureChest>().content = item.GetPrefab();
            }
        }
    }
}
