using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace SortColonistBar.Patches;

[HarmonyPatch(typeof(ColonistBarColonistDrawer), nameof(ColonistBarColonistDrawer.HandleClicks))]
internal class ColonistBarColonistDrawer_HandleClicks_Patches
{
    private static bool mousePressed;

    [HarmonyPrefix]
    public static bool Prefix(Rect rect, Pawn colonist, int reorderableGroup)
    {
#if DEBUG
        if (Event.current.type != EventType.Layout
            && Event.current.button == 0
            && Event.current.type == EventType.MouseDown
            && Event.current.clickCount == 2
            && Mouse.IsOver(rect))
        {
            // Debugging purposes to get some stats
            Log.Message($"{nameof(colonist.LabelCap)}: {colonist.LabelCap}");
            Log.Message($"{nameof(colonist.ThingID)}: {colonist.thingIDNumber}");
            Log.Message(
                $"{nameof(StatDefOf.TradePriceImprovement)}: {colonist.GetStatValue(StatDefOf.TradePriceImprovement)}");
            Log.Message($"{nameof(StatDefOf.MedicalTendSpeed)}: {colonist.GetStatValue(StatDefOf.MedicalTendSpeed)}");
            Log.Message($"{nameof(StatDefOf.MoveSpeed)}: {colonist.GetStatValue(StatDefOf.MoveSpeed)}");
            Log.Message($"{nameof(StatDefOf.MarketValue)}: {colonist.GetStatValue(StatDefOf.MarketValue)}");
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

            if (Event.current.type != EventType.MouseUp
                || !mousePressed
                || !Mouse.IsOver(rect))
            {
                return true;
            }

            Tools.RefreshMenu();
            Find.WindowStack.Add(Tools.LabelMenu);
            return false;
        }

        mousePressed = false;

        return true;
    }
}