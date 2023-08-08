using ReventureRando.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ReventureRando.ItemLocations
{
    public class SwordInGuard : ItemLocation
    {
        public SwordInGuard() : base("World/NPCs/KindomNPCs/Dead_Guard")
        {

        }

        protected override GameObject DisableOldItem()
        {
            GameObject oldItem = GameObject.Find("World/NPCs/KindomNPCs/Dead_Guard/Item Sword");
            GameObject.Destroy(oldItem.GetComponent<AlterWithMilestone>());
            GameObject.Destroy(oldItem.GetComponent<AlterWithRestrictions>());
            GameObject.Destroy(oldItem.GetComponent<AlterWithEnding>());
            oldItem.SetActive(false);
            return oldItem;
        }

        protected override void EnableNewItem(Item item, GameObject oldGameObject)
        {
            SpawnNewItem(item, oldGameObject, "Item at Guard");
        }
    }
}
