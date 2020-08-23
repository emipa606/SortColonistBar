using System;
using UnityEngine;
using Verse;

namespace SortColonistBar.FloatMenus
{
    public class FloatMenuOptionNoClose : FloatMenuOption
    {
        public FloatMenuOptionNoClose(string label, Action action)
        : base(label, action, MenuOptionPriority.Default)
        {
            if (this.action == null)
            {
                this.action = new Action(() => Tools.CloseLabelMenu());
            }
            else
            {
                this.action += new Action(() => Tools.CloseLabelMenu());
            }
        }

        public override bool DoGUI(Rect rect, bool colonistOrdering, FloatMenu floatMenu)
        {
            base.DoGUI(rect, colonistOrdering, floatMenu);
            return false; // don't close after an item is selected
        }
    }
}
