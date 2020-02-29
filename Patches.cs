using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace SortColonistBar.Patches
{
    [StaticConstructorOnStartup]
    internal static class HarmonyPatches
    {
        static HarmonyPatches()
        {
            HarmonyInstance harmony = HarmonyInstance.Create("rimworld.mod.SortColonistBar");

            harmony.Patch(AccessTools.Method(typeof(ColonistBarColonistDrawer), nameof(ColonistBarColonistDrawer.HandleClicks)),
                              new HarmonyMethod(typeof(HandleClicks), nameof(HandleClicks.HandleClicks_Prefix)),
                              null);

            harmony.Patch(AccessTools.Method(typeof(PlayerPawnsDisplayOrderUtility), nameof(PlayerPawnsDisplayOrderUtility.Sort)),
                new HarmonyMethod(typeof(Sort), nameof(Sort.Sort_Prefix)),
                new HarmonyMethod(typeof(Sort), nameof(Sort.Sort_PostFix)));
        }
    }

    internal class Sort
    {
        public static bool Sort_Prefix(ref Func<Pawn, int> ___displayOrderGetter, ref Func<Pawn, int> ___thingIDNumberGetter, ref List<Pawn> pawns)
        {
            switch (Tools.Sort)
            {
                case Tools.SortChoice.Name:
                    return false;

                default:
                    //if (!pawns.NullOrEmpty())
                    {
                        ___displayOrderGetter = Tools.DisplayOrderGetter;
                        ___thingIDNumberGetter = Tools.ThingIDNumberGetter;
                    }
                    break;
            }

            return true;
        }

        public static void Sort_PostFix(ref List<Pawn> pawns)
        {
            if (!pawns.NullOrEmpty())
            {
                if (Tools.Sort == Tools.SortChoice.Name)
                {
                    pawns.SortBy(x => x?.LabelCap);
                }
            }
        }
    }

    internal class HandleClicks
    {
        private static bool mousePressed = false;
        public static bool HandleClicks_Prefix(Rect rect, Pawn colonist, ref int reorderableGroup)
        {
#if DEBUG
            if (Event.current.type != EventType.Layout
                && Event.current.button == 0
                && Event.current.type == EventType.MouseDown
                && Event.current.clickCount == 2
                && Mouse.IsOver(rect))
            {
                // Debugging purposes to get some stats
                Log.Message(nameof(colonist.LabelCap) + ": " + colonist.LabelCap);
                Log.Message(nameof(colonist.ThingID) + ": " + colonist.thingIDNumber);
                Log.Message(nameof(StatDefOf.TradePriceImprovement) + ": " + colonist.GetStatValue(StatDefOf.TradePriceImprovement));
                Log.Message(nameof(StatDefOf.MedicalTendSpeed) + ": " + colonist.GetStatValue(StatDefOf.MedicalTendSpeed));
                Log.Message(nameof(StatDefOf.MoveSpeed) + ": " + colonist.GetStatValue(StatDefOf.MoveSpeed));
                Log.Message(nameof(StatDefOf.MarketValue) + ": " + colonist.GetStatValue(StatDefOf.MarketValue));
            }
#endif
            if (Event.current.type != EventType.MouseDrag
             && Event.current.button == 1
             && Event.current.clickCount == 1)
            {
                if (Event.current.type == EventType.MouseDown)
                {
                    mousePressed = true;
                }
                if (Event.current.type == EventType.MouseUp
                    && mousePressed
                    && Mouse.IsOver(rect))
                {
                    Find.WindowStack.Add(Tools.LabelMenu);
                    return false;
                }
            }
            else
            {
                mousePressed = false;
            }
            return true;
        }
    }
}
