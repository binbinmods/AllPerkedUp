using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
// using static Obeliskial_Essentials.Essentials;
using System;
using static AllPerkedUp.Plugin;
using static AllPerkedUp.CustomFunctions;
using static AllPerkedUp.AllPerkedUpFunctions;
using System.Collections.Generic;
using static Functions;
using UnityEngine;
// using Photon.Pun;
using TMPro;
using System.Linq;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Diagnostics;
// using Unity.TextMeshPro;

// Make sure your namespace is the same everywhere
namespace AllPerkedUp
{

    [HarmonyPatch] // DO NOT REMOVE/CHANGE - This tells your plugin that this is part of the mod

    public class AllPerkedUpPatches
    {
        public static bool devMode = false; //DevMode.Value;
        public static bool bSelectingPerk = false;
        public static bool IsHost()
        {
            return GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster();
        }




        public static bool CanChangePerk()
        {
            bool singleplayerCanChange = EnablePerkChangeWhenever.Value || (EnablePerkChangeInTowns.Value && AtOManager.Instance.CharInTown());
            bool mpCanChange = EnablePerkChangeWhenever.Value || (EnablePerkChangeInTownsMP && AtOManager.Instance.CharInTown());
            return true; //IsHost() ? singleplayerCanChange : mpCanChange;
        }




        [HarmonyPostfix]
        [HarmonyPatch(typeof(PerkTree), "CanModify")]
        public static void CanModifyPostfix(ref bool __result)
        {
            if (CanChangePerk())
                __result = true;
        }


        [HarmonyPrefix]
        [HarmonyPatch(typeof(PerkTree), "SelectPerk")]
        public static void SelectPerkPrefix()
        {
            if (CanChangePerk())
                bSelectingPerk = true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PerkTree), "SelectPerk")]
        public static void SelectPerkPostfix()
        {
            bSelectingPerk = false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(AtOManager), "CharInTown")]
        public static void CharInTownPostfix(ref bool __result)
        {
            if (bSelectingPerk)// && EnablePerkChangeWhenever.Value)
                __result = true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(AtOManager), "GetTownTier")]
        public static void GetTownTierPostfix(ref int __result)
        {
            if (bSelectingPerk)// && EnablePerkChangeWhenever.Value)
                __result = 0;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(SettingsManager), "IsActive")]
        public static void SettingsManagerIsActivePostfix(ref bool __result)
        {
            if (bSelectingPerk)
                __result = false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(AlertManager), "IsActive")]
        public static void AlertManagerIsActivePostfix(ref bool __result)
        {
            if (bSelectingPerk)
                __result = false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(MadnessManager), "IsActive")]
        public static void MadnessManagerIsActivePostfix(ref bool __result)
        {
            if (bSelectingPerk)
                __result = false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PerkNode), "OnMouseUp")]
        public static void OnMouseUpPrefix(ref PerkNode __instance)
        {
            if (CanChangePerk())
            {
                Traverse.Create(__instance).Field("nodeLocked").SetValue(false);
                __instance.iconLock.gameObject.SetActive(false);
                bSelectingPerk = true;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PerkNode), "OnMouseUp")]
        public static void OnMouseUpPostfix()
        {
            bSelectingPerk = false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PerkNode), "OnMouseEnter")]
        public static void OnMouseEnterPrefix(ref PerkNode __instance)
        {
            if (CanChangePerk())
            {
                bSelectingPerk = true;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PerkNode), "OnMouseEnter")]
        public static void OnMouseEnterPostfix()
        {
            bSelectingPerk = false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PerkTree), "Show")]
        public static void ShowPostfix(ref PerkTree __instance, ref int ___totalAvailablePoints)
        {
            if (CanChangePerk())
            {
                __instance.buttonReset.gameObject.SetActive(value: true);
                __instance.buttonImport.gameObject.SetActive(value: true);
                __instance.buttonExport.gameObject.SetActive(value: true);
                __instance.saveSlots.gameObject.SetActive(value: true);
                __instance.buttonConfirm.gameObject.SetActive(value: true);
                // __instance.buttonConfirm.Enable();
            }
            return;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PerkNode), "SetIconLock")]
        public static void SetIconLockPrefix(ref bool _state)
        {
            if (CanChangePerk())
                _state = false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PerkNode), "SetLocked")]
        public static void SetLockedPrefix(ref bool _status)
        {
            if (CanChangePerk())
                _status = false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(SteamManager), "SetObeliskScore")]
        public static bool SetObeliskScorePrefix(ref SteamManager __instance, int score, bool singleplayer = true)
        {
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(SteamManager), "SetScore")]
        public static bool SetScorePrefix(ref SteamManager __instance, int score, bool singleplayer = true)
        {
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(SteamManager), "SetSingularityScore")]
        public static bool SetSingularityScorePrefix(ref SteamManager __instance, int score, bool singleplayer = true)
        {
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(SteamManager), "SetObeliskScoreLeaderboard")]
        public static bool SetObeliskScoreLeaderboardPrefix(ref SteamManager __instance, int score, bool singleplayer = true)
        {
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(SteamManager), "SetScoreLeaderboard")]
        public static bool SetScoreLeaderboardPrefix(ref SteamManager __instance, int score, bool singleplayer = true)
        {
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(SteamManager), "SetSingularityScoreLeaderboard")]
        public static bool SetSingularityScoreLeaderboardPrefix(ref SteamManager __instance, int score, bool singleplayer = true)
        {
            return false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(AtOManager), "GlobalAuraCurseModificationByTraitsAndItems")]
        public static void GlobalAuraCurseModificationByTraitsAndItemsPostfix(ref AtOManager __instance, ref AuraCurseData __result, string _type, string _acId, Character _characterCaster, Character _characterTarget)
        {
            // LogInfo($"GACM MoreMadness");
            Character characterOfInterest = _type == "set" ? _characterTarget : _characterCaster;
            bool isNpcThatGetsPerks = IsLivingNPC(characterOfInterest) && difficultyLevelInt >= (int)DifficultyLevelEnum.Hard && HasCorruptor(Corruptors.Decadence);
            bool isHeroThatGetsPerks = IsLivingHero(characterOfInterest) && difficultyLevelInt >= (int)DifficultyLevelEnum.Hard && HasCorruptor(Corruptors.Decadence);
            switch (_acId)
            {
                case "powerful":
                    if ((devMode || (difficultyLevelInt >= (int)DifficultyLevelEnum.Normal && HasCorruptor(Corruptors.OverchargedMonsters))) && !characterOfInterest.IsHero)
                    {
                        int amountToAdd = Math.Max(10, 5 * difficultyLevelInt);
                        __result.MaxCharges = amountToAdd;
                        __result.MaxMadnessCharges = amountToAdd;
                    }
                    break;
                case "bless":
                    if ((devMode || (difficultyLevelInt >= (int)DifficultyLevelEnum.Extreme && HasCorruptor(Corruptors.Equalizer))) && !characterOfInterest.IsHero && AtOManager.Instance.GetActNumberForText() > 1)
                    {
                        __result.ConsumedAtTurn = false;
                        __result.ConsumedAtTurnBegin = false;
                    }
                    if (isNpcThatGetsPerks)
                    {
                        __result.AuraDamageIncreasedPerStack *= 1.5f;
                        __result = __instance.GlobalAuraCurseModifyResist(__result, Enums.DamageType.Holy, 0, 0.5f);
                        __result.ConsumedAtTurn = false;
                        __result.AuraConsumed = 0;
                    }
                    break;
                case "fury":
                    if ((devMode || (difficultyLevelInt >= (int)DifficultyLevelEnum.Extreme && HasCorruptor(Corruptors.OverchargedMonsters))) && !characterOfInterest.IsHero && AtOManager.Instance.GetActNumberForText() > 1)
                    {
                        __result.MaxCharges = -1;
                        __result.MaxMadnessCharges = -1;
                    }
                    if (isNpcThatGetsPerks)
                    {
                        __result.AuraDamageIncreasedPercentPerStack = 5f;
                    }
                    break;
                case "chill":
                    if (isNpcThatGetsPerks)
                    {
                        __result.CharacterStatChargesMultiplierNeededForOne = 8;
                    }
                    break;

                case "buffer":
                    if (isNpcThatGetsPerks)
                    {
                        __result.GainCharges = true;
                    }
                    break;
                case "evasion":
                    string enchantment1 = "abstraction";
                    if (NpcHaveEnchant(characterOfInterest, enchantment1))
                    {
                        __result.GainCharges = true;
                    }
                    break;
                case "scourge":
                    string enchantment2 = "anathema";
                    if (isHeroThatGetsPerks || NpcHaveEnchant(characterOfInterest, enchantment2))
                    {
                        __result.GainCharges = true;
                    }
                    break;
                case "zeal":
                    string enchantment3 = "apotheosis";
                    if (isNpcThatGetsPerks || NpcHaveEnchant(characterOfInterest, enchantment3))
                    {
                        __result.GainCharges = true;
                    }
                    break;
                case "wet":
                    if (isNpcThatGetsPerks)
                    {
                        __result.IncreasedDamageReceivedType2 = Enums.DamageType.Cold;
                        __result.IncreasedDirectDamageReceivedPerStack2 = 1f;
                    }
                    break;
                case "fortify":
                    if (isNpcThatGetsPerks)
                    {
                        __result.AuraDamageType = Enums.DamageType.Blunt;
                        __result.AuraDamageIncreasedPerStack = 1f;
                        __result.AuraDamageType2 = Enums.DamageType.Fire;
                        __result.AuraDamageIncreasedPerStack2 = 1f;
                        __result.GainCharges = true;
                        __result.MaxCharges = __result.MaxMadnessCharges = 50;
                    }
                    break;
                case "insulate":
                    if (isNpcThatGetsPerks)
                    {
                        __result.ResistModifiedValue = 20;
                        __result.ResistModifiedPercentagePerStack = 5f;
                        __result.ResistModifiedValue2 = 20;
                        __result.ResistModifiedPercentagePerStack2 = 5f;
                        __result.ResistModifiedValue3 = 20;
                        __result.ResistModifiedPercentagePerStack3 = 5f;
                        __result.GainCharges = true;
                        __result.MaxCharges = __result.MaxMadnessCharges = 16;
                    }
                    break;
                case "reinforce":
                    if (isNpcThatGetsPerks)
                    {
                        __result.ResistModifiedValue = 20;
                        __result.ResistModifiedPercentagePerStack = 5f;
                        __result.ResistModifiedValue2 = 20;
                        __result.ResistModifiedPercentagePerStack2 = 5f;
                        __result.ResistModifiedValue3 = 20;
                        __result.ResistModifiedPercentagePerStack3 = 5f;
                        __result.GainCharges = true;
                        __result.MaxCharges = __result.MaxMadnessCharges = 16;
                    }
                    break;
                case "courage":
                    if (isNpcThatGetsPerks)
                    {
                        __result.ResistModifiedValue = 20;
                        __result.ResistModifiedPercentagePerStack = 5f;
                        __result.ResistModifiedValue2 = 20;
                        __result.ResistModifiedPercentagePerStack2 = 5f;
                        __result.ResistModifiedValue3 = 20;
                        __result.ResistModifiedPercentagePerStack3 = 5f;
                        __result.GainCharges = true;
                        __result.MaxCharges = __result.MaxMadnessCharges = 16;
                    }
                    break;
                case "burn":
                    if (isNpcThatGetsPerks)
                    {
                        __result.ResistModifiedValue = 20;
                        __result.ResistModifiedPercentagePerStack = 5f;
                        __result.ResistModifiedValue2 = 20;
                        __result.ResistModifiedPercentagePerStack2 = 5f;
                        __result.ResistModifiedValue3 = 20;
                        __result.ResistModifiedPercentagePerStack3 = 5f;
                        __result.GainCharges = true;
                        __result.MaxCharges = __result.MaxMadnessCharges = 10;
                    }
                    break;
                case "bleed":
                    if (isNpcThatGetsPerks)
                    {
                        __result.DamageWhenConsumedPerCharge *= 1.5f;
                    }
                    break;
                case "sharp":
                    if (isNpcThatGetsPerks)
                    {
                        __result.AuraDamageIncreasedPerStack *= 1.5f;
                        __result.AuraDamageIncreasedPerStack2 *= 1.5f;
                    }
                    break;
                case "decay":
                    if (isNpcThatGetsPerks)
                    {
                        __result = __instance.GlobalAuraCurseModifyResist(__result, Enums.DamageType.Shadow, 0, -8f);
                    }
                    break;
                case "mark":
                    if (isNpcThatGetsPerks)
                    {
                        __result.ConsumedAtTurn = false;
                        __result.AuraConsumed = 0;
                        __result = __instance.GlobalAuraCurseModifyResist(__result, Enums.DamageType.Slashing, 0, -0.3f);
                    }
                    break;
                case "poison":
                    if (isHeroThatGetsPerks)
                    {
                        __result.ResistModified = Enums.DamageType.Shadow;
                        __result.ResistModifiedPercentagePerStack = characterOfInterest.HasEffect("rust") ? -0.45f : -0.3f;
                    }
                    break;
                case "regeneration":
                    if (isNpcThatGetsPerks)
                    {
                        __result.HealSidesWhenConsumed = __result.HealWhenConsumed;
                        __result.HealSidesWhenConsumedPerCharge = __result.HealWhenConsumedPerCharge;
                        __result = __instance.GlobalAuraCurseModifyResist(__result, Enums.DamageType.Shadow, 0, 0.5f);
                        __result.AuraConsumed = 0;
                    }
                    break;
                case "thorns":
                    if (isNpcThatGetsPerks)
                    {
                        __result.Removable = false;
                        __result.DamageReflectedConsumeCharges = 0;
                    }
                    break;
                case "sanctify":
                    if (isNpcThatGetsPerks)
                    {
                        __result = __instance.GlobalAuraCurseModifyResist(__result, Enums.DamageType.Holy, 0, -0.5f);
                        break;
                    }
                    break;
                case "stealth":
                    if (isNpcThatGetsPerks)
                    {
                        __result.AuraDamageIncreasedPercentPerStack = 25f;
                        __result.HealDonePercentPerStack = 25;
                        __result.ConsumedAtTurnBegin = false;
                        __result.AuraConsumed = 0;
                        __result = __instance.GlobalAuraCurseModifyResist(__result, Enums.DamageType.All, 0, 5f);
                    }
                    break;
                case "stealthbonus":
                    if (isNpcThatGetsPerks)
                    {
                        __result.AuraDamageIncreasedPercentPerStack = 25f;
                        __result.HealDonePercentPerStack = 25;

                    }
                    break;
                case "taunt":
                    if (isNpcThatGetsPerks)
                    {
                        __result.ResistModified = Enums.DamageType.All;
                        __result.ResistModifiedPercentagePerStack = 10f;
                    }
                    break;
                case "vitality":
                    if (isNpcThatGetsPerks)
                    {
                        __result = __instance.GlobalAuraCurseModifyResist(__result, Enums.DamageType.Mind, 0, 0.5f);
                        __result.ConsumedAtTurnBegin = false;
                        __result.AuraConsumed = 0;
                        __result.CharacterStatModifiedValuePerStack = 8;
                    }
                    break;
            }
        }



    }
}