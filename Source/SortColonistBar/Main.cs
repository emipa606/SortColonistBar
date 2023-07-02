using System.Reflection;
using HarmonyLib;
using Verse;

namespace SortColonistBar.Patches;

[StaticConstructorOnStartup]
public class Main
{
    static Main()
    {
        new Harmony("rimworld.mod.sortcolonistbar").PatchAll(Assembly.GetExecutingAssembly());
    }
}