using ReventureRando.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ReventureRando.ItemLocations
{
    public class PrincessLocation : ItemLocation
    {
        public PrincessLocation() : base("World/NPCs/")
        {

        }

        protected override GameObject DisableOldItem()
        {
            GameObject princess = GameObject.Find("World/NPCs/Item Princess");
            princess.SetActive(false);
            return princess;
        }

        protected override void EnableNewItem(Item item, GameObject oldGameObject)
        {
            SpawnNewItem(item, oldGameObject, "Princess Item");
        }
    }
}
