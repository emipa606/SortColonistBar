using System.Collections.Generic;
using Verse;

namespace SortColonistBar.FloatMenus;

public class FloatMenuLabels : FloatMenu
{
    protected FloatMenuLabels(List<FloatMenuOption> options, string title = "") : base(options, title)
    {
        givesColonistOrders = false;
        vanishIfMouseDistant = true;
        closeOnClickedOutside = true;
    }
}