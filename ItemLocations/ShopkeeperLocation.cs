using Atto;
using ReventureRando.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ReventureRando.ItemLocations
{
    public class ShopkeeperLocation : ItemLocation
    {
        bool shopKeeperKilledOnce;

        public ShopkeeperLocation() : base("World/NPCs/Shopkeepers/ArmedShopkeeper/ArmedShopkeeper")
        {
            shopKeeperKilledOnce = Core.Get<IProgressionService>().IsEndingUnlocked(EndingTypes.StabShopKeeper) && Core.Get<IProgressionService>().IsEndingUnlocked(EndingTypes.EatenByFakePrincess);
            GameObject unarmedShopkeeper = GameObject.Find("World/NPCs/Shopkeepers/UnarmedShopkeeper");
            if (unarmedShopkeeper != null)
            {
                unarmedShopkeeper.SetActive(false);
            }
        }

        protected override GameObject DisableOldItem()
        {
                return null;
        }

        protected override void EnableNewItem(Item item, GameObject oldGameObject)
        {
            if (!shopKeeperKilledOnce)
            {
                return;
            }
            NPC shopkeeper = gameObject.GetComponent<NPC>();
            if (item == null)
            {
                shopkeeper.spawnOnKill = null;
            }
            shopkeeper.spawnOnKill = item.GetPrefab();
        }
    }
}
