using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace SortColonistBar.FloatMenus
{
    public class FloatMenuLabels : FloatMenu
    {
        public FloatMenuLabels(List<Verse.FloatMenuOption> options, string title = "") : base(options, title, false)
        {
            givesColonistOrders = false;
            vanishIfMouseDistant = true;
            closeOnClickedOutside = true;
        }
    }
}

