using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace SortColonistBar.Patches;

[HarmonyPatch(typeof(PlayerPawnsDisplayOrderUtility))]
[HarmonyPatch("Sort")]
internal class PlayerPawnsDisplayOrderUtility_Sort_Patch
{
    [HarmonyPrefix]
    public static bool Sort_Prefix(ref Func<Pawn, int> ___displayOrderGetter)
    {
        switch (Tools.Sort)
        {
            case Tools.SortChoice.Name:
                return false;

            default:
            {
                ___displayOrderGetter = Tools.DisplayOrderGetter;
                //___thingIDNumberGetter = Tools.ThingIDNumberGetter;
            }
                break;
        }

        return true;
    }

    [HarmonyPostfix]
    public static void Sort_PostFix(ref List<Pawn> pawns)
    {
        if (pawns.NullOrEmpty())
        {
            return;
        }

        if (Tools.Sort == Tools.SortChoice.Name)
        {
            pawns.SortBy(x => x?.LabelCap);
        }

        if (Tools.Reverse)
        {
            pawns.Reverse();
        }
    }
}