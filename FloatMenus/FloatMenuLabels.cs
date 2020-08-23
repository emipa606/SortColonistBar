using System.Collections.Generic;
using Verse;

namespace SortColonistBar.FloatMenus
{
    public class FloatMenuLabels : FloatMenu
    {
        public FloatMenuLabels(List<FloatMenuOption> options, string title = "") : base(options, title, false)
        {
            givesColonistOrders = false;
            vanishIfMouseDistant = true;
            closeOnClickedOutside = true;
        }
    }
}

