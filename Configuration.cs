using BepInEx;
using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReventureRando
{
    public class Configuration
    {
        public ConfigEntry<int> freeEndingCount;
        public ConfigEntry<string> availableLocationsBlacklist;
        public ConfigEntry<string> availableItemsBlacklist;

        public Configuration(BaseUnityPlugin plugin)
        {
            var Config = plugin.Config;


            //Create Configuration
            freeEndingCount = Config.Bind("Randomizer",      // The section under which the option is shown
                                             "FreeEndingCount",  // The key of the configuration option in the configuration file
                                             70, // The default value
                                             "Amount of Endings to autounlock when creating a new save"); // Description of the option to show in the config file

            availableLocationsBlacklist = Config.Bind("Randomizer",
                                                        "DisabledLocations",
                                                        "TreasureRoom,TreasureChest_MyPhone,Shopkeeper,DarkChickenLocation",
                                                        "These Locations will not be randomized");

            availableItemsBlacklist = Config.Bind("Randomizer",
                                                  "DisabledItems",
                                                  "MyPhone,Shotgun",
                                                  "These Items will not be randomized");
        }
    }
}
