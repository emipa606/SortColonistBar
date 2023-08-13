using System;
using System.Collections.Generic;
using RimWorld;
using SortColonistBar.FloatMenus;
using Verse;

namespace SortColonistBar;

public static class Tools
{
    public enum SortChoice
    {
        Manual,
        Name,
        Reverse,
        Age,
        Ranged,
        Melee,
        Construction,
        Mining,
        Cooking,
        Plants,
        Animals,
        Crafting,
        Art,
        Health,
        Medicine,
        Social,
        Intellectual,
        Speed,
        Value
    }

    private const int _noSkill = 999999;
    private const int _oneDigitSignificant = 1000;
    private const int maxSkillLevel = 20;

    public static readonly FloatMenuWithOptions LabelMenu =
        new FloatMenuWithOptions(new List<FloatMenuOption>
        {
            MakeMenuItemForLabel(SortChoice.Manual),
            MakeMenuItemForLabel(SortChoice.Name),
            MakeMenuItemForLabel(SortChoice.Reverse),
            MakeMenuItemForLabel(SortChoice.Age),
            MakeMenuItemForLabel(SortChoice.Ranged),
            MakeMenuItemForLabel(SortChoice.Melee),
            MakeMenuItemForLabel(SortChoice.Construction),
            MakeMenuItemForLabel(SortChoice.Mining),
            MakeMenuItemForLabel(SortChoice.Cooking),
            MakeMenuItemForLabel(SortChoice.Plants),
            MakeMenuItemForLabel(SortChoice.Animals),
            MakeMenuItemForLabel(SortChoice.Crafting),
            MakeMenuItemForLabel(SortChoice.Art),
            MakeMenuItemForLabel(SortChoice.Medicine),
            MakeMenuItemForLabel(SortChoice.Social),
            MakeMenuItemForLabel(SortChoice.Intellectual),
            MakeMenuItemForLabel(SortChoice.Value),
            MakeMenuItemForLabel(SortChoice.Health),
            MakeMenuItemForLabel(SortChoice.Speed)
        });

    private static SortChoice _sort = SortChoice.Manual;

    private static readonly Func<Pawn, int> _defaultDisplayOrderGetter =
        x => x.playerSettings?.displayOrder ?? _noSkill;

    private static readonly Func<Pawn, int> _defaultThingIDNumberGetter = x => x.thingIDNumber;

    public static bool Reverse;

    public static SortChoice Sort
    {
        get => _sort;
        set
        {
#if DEBUG
            Log.Message($"Sort: {value}");
#endif
            Find.ColonistBar.MarkColonistsDirty();
            if (_sort != value)
            {
                ThingIDNumberGetter = NextThingIDNumberGetter;
            }

            switch (value)
            {
                case SortChoice.Name:
                    break;
                case SortChoice.Reverse:
                    Reverse = !Reverse;
                    return;
                case SortChoice.Manual:
                    DisplayOrderGetter = _defaultDisplayOrderGetter;
                    ThingIDNumberGetter = _defaultThingIDNumberGetter;
                    break;
                case SortChoice.Age:
                    DisplayOrderGetter = x =>
                        -x.ageTracker.AgeBiologicalYears;
                    NextThingIDNumberGetter = x =>
                        x.ageTracker.AgeBiologicalYears;
                    break;
                case SortChoice.Speed:
                    DisplayOrderGetter = x => -(int)(x.GetStatValue(StatDefOf.MoveSpeed) * _oneDigitSignificant);
                    NextThingIDNumberGetter = x =>
                        _oneDigitSignificant - (int)(x.GetStatValue(StatDefOf.MoveSpeed) * _oneDigitSignificant);
                    break;
                case SortChoice.Value:
                    DisplayOrderGetter = x => -(int)x.GetStatValue(StatDefOf.MarketValue);
                    NextThingIDNumberGetter = x => _noSkill - (int)x.GetStatValue(StatDefOf.MarketValue);
                    break;
                case SortChoice.Social:
                    DisplayOrderGetter = x =>
                        x.skills?.GetSkill(SkillDefOf.Social) == null ||
                        x.skills.GetSkill(SkillDefOf.Social).TotallyDisabled
                            ? _noSkill
                            : -x.skills.GetSkill(SkillDefOf.Social).Level;
                    NextThingIDNumberGetter = x =>
                        x.skills?.GetSkill(SkillDefOf.Social) == null ||
                        x.skills.GetSkill(SkillDefOf.Social).TotallyDisabled
                            ? _noSkill
                            : maxSkillLevel - x.skills.GetSkill(SkillDefOf.Social).Level;
                    break;
                case SortChoice.Ranged:
                    DisplayOrderGetter = x =>
                        x.skills?.GetSkill(SkillDefOf.Shooting) == null ||
                        x.skills.GetSkill(SkillDefOf.Shooting).TotallyDisabled
                            ? _noSkill
                            : -x.skills.GetSkill(SkillDefOf.Shooting).Level;
                    NextThingIDNumberGetter = x =>
                        x.skills?.GetSkill(SkillDefOf.Shooting) == null ||
                        x.skills.GetSkill(SkillDefOf.Shooting).TotallyDisabled
                            ? _noSkill
                            : maxSkillLevel - x.skills.GetSkill(SkillDefOf.Shooting).Level;
                    break;
                case SortChoice.Medicine:
                    DisplayOrderGetter = x =>
                        x.skills?.GetSkill(SkillDefOf.Medicine) == null ||
                        x.skills.GetSkill(SkillDefOf.Medicine).TotallyDisabled
                            ? _noSkill
                            : -x.skills.GetSkill(SkillDefOf.Medicine).Level;
                    NextThingIDNumberGetter = x =>
                        x.skills?.GetSkill(SkillDefOf.Medicine) == null ||
                        x.skills.GetSkill(SkillDefOf.Medicine).TotallyDisabled
                            ? x.thingIDNumber
                            : maxSkillLevel - x.skills.GetSkill(SkillDefOf.Medicine).Level;
                    break;
                case SortChoice.Crafting:
                    DisplayOrderGetter = x =>
                        x.skills?.GetSkill(SkillDefOf.Crafting) == null ||
                        x.skills.GetSkill(SkillDefOf.Crafting).TotallyDisabled
                            ? _noSkill
                            : -x.skills.GetSkill(SkillDefOf.Crafting).Level;
                    NextThingIDNumberGetter = x =>
                        x.skills?.GetSkill(SkillDefOf.Crafting) == null ||
                        x.skills.GetSkill(SkillDefOf.Crafting).TotallyDisabled
                            ? x.thingIDNumber
                            : maxSkillLevel - x.skills.GetSkill(SkillDefOf.Crafting).Level;
                    break;
                case SortChoice.Construction:
                    DisplayOrderGetter = x =>
                        x.skills?.GetSkill(SkillDefOf.Construction) == null ||
                        x.skills.GetSkill(SkillDefOf.Construction).TotallyDisabled
                            ? _noSkill
                            : -x.skills.GetSkill(SkillDefOf.Construction).Level;
                    NextThingIDNumberGetter = x =>
                        x.skills?.GetSkill(SkillDefOf.Construction) == null ||
                        x.skills.GetSkill(SkillDefOf.Construction).TotallyDisabled
                            ? x.thingIDNumber
                            : maxSkillLevel - x.skills.GetSkill(SkillDefOf.Construction).Level;
                    break;
                case SortChoice.Cooking:
                    DisplayOrderGetter = x =>
                        x.skills?.GetSkill(SkillDefOf.Cooking) == null ||
                        x.skills.GetSkill(SkillDefOf.Cooking).TotallyDisabled
                            ? _noSkill
                            : -x.skills.GetSkill(SkillDefOf.Cooking).Level;
                    NextThingIDNumberGetter = x =>
                        x.skills?.GetSkill(SkillDefOf.Cooking) == null ||
                        x.skills.GetSkill(SkillDefOf.Cooking).TotallyDisabled
                            ? x.thingIDNumber
                            : maxSkillLevel - x.skills.GetSkill(SkillDefOf.Cooking).Level;
                    break;
                case SortChoice.Mining:
                    DisplayOrderGetter = x =>
                        x.skills?.GetSkill(SkillDefOf.Mining) == null ||
                        x.skills.GetSkill(SkillDefOf.Mining).TotallyDisabled
                            ? _noSkill
                            : -x.skills.GetSkill(SkillDefOf.Mining).Level;
                    NextThingIDNumberGetter = x =>
                        x.skills?.GetSkill(SkillDefOf.Mining) == null ||
                        x.skills.GetSkill(SkillDefOf.Mining).TotallyDisabled
                            ? x.thingIDNumber
                            : maxSkillLevel - x.skills.GetSkill(SkillDefOf.Mining).Level;
                    break;
                case SortChoice.Intellectual:
                    DisplayOrderGetter = x =>
                        x.skills?.GetSkill(SkillDefOf.Intellectual) == null ||
                        x.skills.GetSkill(SkillDefOf.Intellectual).TotallyDisabled
                            ? _noSkill
                            : -x.skills.GetSkill(SkillDefOf.Intellectual).Level;
                    NextThingIDNumberGetter = x =>
                        x.skills?.GetSkill(SkillDefOf.Intellectual) == null ||
                        x.skills.GetSkill(SkillDefOf.Intellectual).TotallyDisabled
                            ? _noSkill
                            : maxSkillLevel - x.skills.GetSkill(SkillDefOf.Intellectual).Level;
                    break;
                case SortChoice.Plants:
                    DisplayOrderGetter = x =>
                        x.skills?.GetSkill(SkillDefOf.Plants) == null ||
                        x.skills.GetSkill(SkillDefOf.Plants).TotallyDisabled
                            ? _noSkill
                            : -x.skills.GetSkill(SkillDefOf.Plants).Level;
                    NextThingIDNumberGetter = x =>
                        x.skills?.GetSkill(SkillDefOf.Plants) == null ||
                        x.skills.GetSkill(SkillDefOf.Plants).TotallyDisabled
                            ? x.thingIDNumber
                            : 20 - x.skills.GetSkill(SkillDefOf.Plants).Level;
                    break;
                case SortChoice.Animals:
                    DisplayOrderGetter = x =>
                        x.skills?.GetSkill(SkillDefOf.Animals) == null ||
                        x.skills.GetSkill(SkillDefOf.Animals).TotallyDisabled
                            ? _noSkill
                            : -x.skills.GetSkill(SkillDefOf.Animals).Level;
                    NextThingIDNumberGetter = x =>
                        x.skills?.GetSkill(SkillDefOf.Animals) == null ||
                        x.skills.GetSkill(SkillDefOf.Animals).TotallyDisabled
                            ? x.thingIDNumber
                            : 20 - x.skills.GetSkill(SkillDefOf.Animals).Level;
                    break;
                case SortChoice.Melee:
                    DisplayOrderGetter = x =>
                        x.skills?.GetSkill(SkillDefOf.Melee) == null ||
                        x.skills.GetSkill(SkillDefOf.Melee).TotallyDisabled
                            ? _noSkill
                            : -x.skills.GetSkill(SkillDefOf.Melee).Level;
                    NextThingIDNumberGetter = x =>
                        x.skills?.GetSkill(SkillDefOf.Melee) == null ||
                        x.skills.GetSkill(SkillDefOf.Melee).TotallyDisabled
                            ? x.thingIDNumber
                            : 20 - x.skills.GetSkill(SkillDefOf.Melee).Level;
                    break;
                case SortChoice.Art:
                    DisplayOrderGetter = x =>
                        x.skills?.GetSkill(SkillDefOf.Artistic) == null ||
                        x.skills.GetSkill(SkillDefOf.Artistic).TotallyDisabled
                            ? _noSkill
                            : -x.skills.GetSkill(SkillDefOf.Artistic).Level;
                    NextThingIDNumberGetter = x =>
                        x.skills?.GetSkill(SkillDefOf.Artistic) == null ||
                        x.skills.GetSkill(SkillDefOf.Artistic).TotallyDisabled
                            ? x.thingIDNumber
                            : 20 - x.skills.GetSkill(SkillDefOf.Artistic).Level;
                    break;
                default:
                    Log.Warning("Unimplemented sort option");
                    _sort = SortChoice.Manual;
                    DisplayOrderGetter = _defaultDisplayOrderGetter;
                    ThingIDNumberGetter = _defaultThingIDNumberGetter;
                    NextThingIDNumberGetter = _defaultThingIDNumberGetter;
                    break;
            }

            Reverse = false;
            _sort = value;
        }
    }

    public static Func<Pawn, int> DisplayOrderGetter { get; private set; } = _defaultDisplayOrderGetter;
    public static Func<Pawn, int> ThingIDNumberGetter { get; private set; } = _defaultThingIDNumberGetter;
    public static Func<Pawn, int> NextThingIDNumberGetter { get; private set; } = _defaultThingIDNumberGetter;

    public static void CloseLabelMenu(bool sound = false)
    {
        if (LabelMenu != null)
        {
            Find.WindowStack.TryRemove(LabelMenu, sound);
        }
    }

    public static FloatMenuOption MakeMenuItemForLabel(SortChoice choice)
    {
        return new FloatMenuOptionNoClose(choice.ToString(), () => Sort = choice);
    }
}