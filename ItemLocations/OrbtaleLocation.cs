using ReventureRando.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ReventureRando.ItemLocations
{
    public class OrbtaleLocation : ItemLocation
    {
        public OrbtaleLocation() : base("World/Items")
        {

        }

        protected override GameObject DisableOldItem()
        {
            GameObject covid = GameObject.Find("World/Items/Item COVID-19");
            covid.SetActive(false);
            return covid;
        }

        protected override void EnableNewItem(Item item, GameObject oldGameObject)
        {
            SpawnNewItem(item, oldGameObject, "Covid Item");
        }
    }
}
