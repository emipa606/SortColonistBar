using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace SortColonistBar.FloatMenus;

public class FloatMenuWithOptions(List<FloatMenuOption> options) : FloatMenuLabels(options)
{
    public override void DoWindowContents(Rect rect)
    {
        options.ForEach(o => { o.SetSizeMode(FloatMenuSizeMode.Normal); });
        windowRect = new Rect(windowRect.x, windowRect.y, InitialSize.x, InitialSize.y);
        base.DoWindowContents(windowRect);
    }

    public override void PostClose()
    {
        base.PostClose();
        Tools.CloseLabelMenu();
    }
}