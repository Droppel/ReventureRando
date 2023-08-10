using ReventureRando.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ReventureRando.ItemLocations
{
    public class ChickenLocation : ItemLocation
    {
        private string chickenname;

        public ChickenLocation(string name, string chickenname) : base(name)
        {
            this.chickenname = chickenname;
        }

        protected override GameObject DisableOldItem()
        {
            GameObject chicken = gameObject.transform.Find("Item Chicken").gameObject;
            chicken.SetActive(false);
            return chicken;
        }

        protected override void EnableNewItem(Item item, GameObject oldGameObject)
        {
            SpawnNewItem(item, oldGameObject, chickenname + " Item");
        }
    }
}
