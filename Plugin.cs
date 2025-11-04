// These are your imports, mostly you'll be needing these 5 for every plugin. Some will need more.

using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using static Obeliskial_Essentials.Essentials;
using static Obeliskial_Essentials.CardDescriptionNew;
using BepInEx.Bootstrap;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
// using static AllPerkedUp.EssentialsCompatibility;


// The Plugin csharp file is used to specify some general info about your plugin. and set up things for 


// Make sure all your files have the same namespace and this namespace matches the RootNamespace in the .csproj file
// All files that are in the same namespace are compiled together and can "see" each other more easily.

namespace AllPerkedUp
{
    // These are used to create the actual plugin. If you don't need Obeliskial Essentials for your mod, 
    // delete the BepInDependency and the associated code "RegisterMod()" below.

    // If you have other dependencies, such as obeliskial content, make sure to include them here.
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("com.stiffmeds.obeliskialessentials", BepInDependency.DependencyFlags.SoftDependency)] // this is the name of the .dll in the !libs folder.
    [BepInProcess("AcrossTheObelisk.exe")] //Don't change this

    // If PluginInfo isn't working, you are either:
    // 1. Using BepInEx v6
    // 2. Have an issue with your csproj file (not loading the analyzer or BepInEx appropriately)
    // 3. You have an issue with your solution file (not referencing the correct csproj file)


    public class Plugin : BaseUnityPlugin
    {

        // If desired, you can create configs for users by creating a ConfigEntry object here, 
        // Configs allows users to specify certain things about the mod. 
        // The most common would be a flag to enable/disable portions of the mod or the entire mod.

        // You can use: config = Config.Bind() to set the title, default value, and description of the config.
        // It automatically creates the appropriate configs.


        public static ConfigEntry<bool> EnableMod { get; set; }
        public static ConfigEntry<bool> EnableDebugging { get; set; }
        // public static ConfigEntry<bool> EnablePerkChangeInTowns { get; set; }
        // public static ConfigEntry<bool> EnablePerkChangeWhenever { get; set; }

        public static string PluginName;
        public static string PluginVersion;
        public static string PluginGUID;

        internal static int ModDate = int.Parse(DateTime.Today.ToString("yyyyMMdd"));
        private readonly Harmony harmony = new(PluginInfo.PLUGIN_GUID);
        internal static ManualLogSource Log;


        public static string debugBase = $"{PluginInfo.PLUGIN_GUID} ";

        private void Awake()
        {

            // The Logger will allow you to print things to the LogOutput (found in the BepInEx directory)
            Log = Logger;

            // Sets the title, default values, and descriptions
            string modName = "AllPerkedUp";
            EnableMod = Config.Bind(new ConfigDefinition(modName, "EnableMod"), true, new ConfigDescription("Enables the mod. If false, the mod will not work then next time you load the game."));
            EnableDebugging = Config.Bind(new ConfigDefinition(modName, "EnableDebugging"), false, new ConfigDescription("Enables the debugging"));
            // EnablePerkChangeInTowns = Config.Bind(new ConfigDefinition(modName, "EnablePerkChangeInTowns"), true, new ConfigDescription("Enables you to change perks in any town."));
            // DevMode = Config.Bind(new ConfigDefinition("DespairMode", "DevMode"), false, new ConfigDescription("Enables all of the things for testing."));
            AcceptableValueList<string> difficulties = new AcceptableValueList<string>(["Off", "Easy", "Normal", "Hard", "Extreme", "Despair"]);
            DifficultyLevel = Config.Bind(new ConfigDefinition("DespairMode", "DifficultyLevel"), "Despair", new ConfigDescription("Sets the difficulty level. Certain features will be enabled/disabled at different difficulties. Restart the game after changing this option to refresh text.", difficulties));
            difficultyLevel = DifficultyLevel.Value switch
            {
                "Off" => DifficultyLevelEnum.Off,
                "Easy" => DifficultyLevelEnum.Easy,
                "Normal" => DifficultyLevelEnum.Normal,
                "Hard" => DifficultyLevelEnum.Hard,
                "Extreme" => DifficultyLevelEnum.Extreme,
                "Despair" => DifficultyLevelEnum.Despair,
                _ => DifficultyLevelEnum.Despair
            };
            difficultyLevelInt = (int)difficultyLevel;

            EnablePerkChangeInTowns = Config.Bind(new ConfigDefinition(modName, "EnablePerkChangeInTowns"), true, new ConfigDescription("Enables you to change perks in any town."));
            EnablePerkChangeInTownsMP = true; // = Config.Bind(new ConfigDefinition(modName, "EnablePerkChangeInTownsMP"), true, new ConfigDescription("Enables you to change perks in any town for multiplayer."));
            EnablePerkChangeWhenever = Config.Bind(new ConfigDefinition(modName, "EnablePerkChangeWhenever"), false, new ConfigDescription("Enables you to change perks at any time."));
            EnablePerkChangeWheneverMP = true; //Config.Bind(new ConfigDefinition(modName, "EnablePerkChangeWheneverMP"), false, new ConfigDescription("Enables you to change perks at any time for Multiplayer."));

            PluginName = PluginInfo.PLUGIN_NAME;
            PluginVersion = PluginInfo.PLUGIN_VERSION;
            PluginGUID = PluginInfo.PLUGIN_GUID;
            if (EnableMod.Value)
            {
                if (EssentialsCompatibility.Enabled)
                    EssentialsCompatibility.EssentialsRegister();
                else
                    LogInfo($"{PluginGUID} {PluginVersion} has loaded!");
                harmony.PatchAll();
            }
        }


        // These are some functions to make debugging a tiny bit easier.
        internal static void LogDebug(string msg)
        {
            if (EnableDebugging.Value)
            {
                Log.LogDebug(debugBase + msg);
            }

        }
        internal static void LogInfo(string msg)
        {
            Log.LogInfo(debugBase + msg);
        }
        internal static void LogError(string msg)
        {
            Log.LogError(debugBase + msg);
        }


    }
}