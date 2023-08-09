using ReventureRando.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ReventureRando.ItemLocations
{
    [Serializable]
    public abstract class ItemLocation
    {
        protected GameObject gameObject;

        public ItemLocation(string name)
        {
            gameObject = GameObject.Find(name);
            SetActiveAll(true);
        }

        private void SetActiveAll(bool state)
        {
            Transform parent = gameObject.transform;
            while (parent != null)
            {
                parent.gameObject.SetActive(state);
                parent = parent.parent;
            }
        }

        public void ReplaceItem(Item item)
        {
            GameObject oldGameObject = DisableOldItem();
            EnableNewItem(item, oldGameObject);
        }

        public GameObject SpawnNewItem(Item item, GameObject oldGameObject, string name)
        {
            if (item == null)
            {
                return null;
            }
            GameObject newItemSpawn = GameObject.Instantiate<GameObject>(item.GetPrefab(), gameObject.transform);
            newItemSpawn.name = name;
            newItemSpawn.transform.position = oldGameObject.transform.position;
            newItemSpawn.GetComponent<BoxCollider2D>().size = oldGameObject.GetComponent<BoxCollider2D>().size;
            newItemSpawn.SetActive(true);
            return newItemSpawn;
        }

        protected abstract GameObject DisableOldItem();
        protected abstract void EnableNewItem(Item item, GameObject oldGameObject);
    }

    public enum ItemLocationEnum
    {
        SwordPedestal,
        SwordAtHome,
        TreasureRoom,
        DeadGuard,
        TreasureChest_Bomb,
        TreasureChest_WhistleOfTime,
        TreasureChest_Hook,
        TreasureChest_MrHugs,
        TreasureChest_Shovel,
        TreasureChest_Trinket,
        TreasureChest_Cannonball,
        TreasureChest_FishingRod,
        TreasureChest_Shield,
        TreasureChest_MyPhone,
        TreasureChest_Compass,
        TreasureChest_Map,
        TreasureChest_Pizza,
        TreasureChest_DarkStone,
        AnvilRope,
        Princess,
        OrbtaleLocation,
        Shopkeeper,
        BoomerangLocation,
    }
}
