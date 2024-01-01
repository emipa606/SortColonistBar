using System;
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
        Food,
        Recreation,
        Sleep,
        Value,
        Mood
    }

    private const int _noSkill = 999999;
    private const int _oneDigitSignificant = 1000;
    private const int maxSkillLevel = 20;

    public static readonly FloatMenuWithOptions LabelMenu =
        new FloatMenuWithOptions([
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
            MakeMenuItemForLabel(SortChoice.Speed),
            MakeMenuItemForLabel(SortChoice.Food),
            MakeMenuItemForLabel(SortChoice.Sleep),
            MakeMenuItemForLabel(SortChoice.Recreation),
            MakeMenuItemForLabel(SortChoice.Mood)
        ]);

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
                    DisplayOrderGetter = pawn =>
                        -pawn.ageTracker.AgeBiologicalYears;
                    NextThingIDNumberGetter = pawn =>
                        pawn.ageTracker.AgeBiologicalYears;
                    break;
                case SortChoice.Speed:
                    DisplayOrderGetter = pawn => -(int)(pawn.GetStatValue(StatDefOf.MoveSpeed) * _oneDigitSignificant);
                    NextThingIDNumberGetter = pawn =>
                        _oneDigitSignificant - (int)(pawn.GetStatValue(StatDefOf.MoveSpeed) * _oneDigitSignificant);
                    break;
                case SortChoice.Value:
                    DisplayOrderGetter = pawn => -(int)pawn.GetStatValue(StatDefOf.MarketValue);
                    NextThingIDNumberGetter = pawn => _noSkill - (int)pawn.GetStatValue(StatDefOf.MarketValue);
                    break;
                case SortChoice.Social:
                    DisplayOrderGetter = pawn =>
                        pawn.skills?.GetSkill(SkillDefOf.Social) == null ||
                        pawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled
                            ? _noSkill
                            : -pawn.skills.GetSkill(SkillDefOf.Social).Level;
                    NextThingIDNumberGetter = pawn =>
                        pawn.skills?.GetSkill(SkillDefOf.Social) == null ||
                        pawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled
                            ? _noSkill
                            : maxSkillLevel - pawn.skills.GetSkill(SkillDefOf.Social).Level;
                    break;
                case SortChoice.Ranged:
                    DisplayOrderGetter = pawn =>
                        pawn.skills?.GetSkill(SkillDefOf.Shooting) == null ||
                        pawn.skills.GetSkill(SkillDefOf.Shooting).TotallyDisabled
                            ? _noSkill
                            : -pawn.skills.GetSkill(SkillDefOf.Shooting).Level;
                    NextThingIDNumberGetter = pawn =>
                        pawn.skills?.GetSkill(SkillDefOf.Shooting) == null ||
                        pawn.skills.GetSkill(SkillDefOf.Shooting).TotallyDisabled
                            ? _noSkill
                            : maxSkillLevel - pawn.skills.GetSkill(SkillDefOf.Shooting).Level;
                    break;
                case SortChoice.Medicine:
                    DisplayOrderGetter = pawn =>
                        pawn.skills?.GetSkill(SkillDefOf.Medicine) == null ||
                        pawn.skills.GetSkill(SkillDefOf.Medicine).TotallyDisabled
                            ? _noSkill
                            : -pawn.skills.GetSkill(SkillDefOf.Medicine).Level;
                    NextThingIDNumberGetter = pawn =>
                        pawn.skills?.GetSkill(SkillDefOf.Medicine) == null ||
                        pawn.skills.GetSkill(SkillDefOf.Medicine).TotallyDisabled
                            ? pawn.thingIDNumber
                            : maxSkillLevel - pawn.skills.GetSkill(SkillDefOf.Medicine).Level;
                    break;
                case SortChoice.Crafting:
                    DisplayOrderGetter = pawn =>
                        pawn.skills?.GetSkill(SkillDefOf.Crafting) == null ||
                        pawn.skills.GetSkill(SkillDefOf.Crafting).TotallyDisabled
                            ? _noSkill
                            : -pawn.skills.GetSkill(SkillDefOf.Crafting).Level;
                    NextThingIDNumberGetter = pawn =>
                        pawn.skills?.GetSkill(SkillDefOf.Crafting) == null ||
                        pawn.skills.GetSkill(SkillDefOf.Crafting).TotallyDisabled
                            ? pawn.thingIDNumber
                            : maxSkillLevel - pawn.skills.GetSkill(SkillDefOf.Crafting).Level;
                    break;
                case SortChoice.Construction:
                    DisplayOrderGetter = pawn =>
                        pawn.skills?.GetSkill(SkillDefOf.Construction) == null ||
                        pawn.skills.GetSkill(SkillDefOf.Construction).TotallyDisabled
                            ? _noSkill
                            : -pawn.skills.GetSkill(SkillDefOf.Construction).Level;
                    NextThingIDNumberGetter = pawn =>
                        pawn.skills?.GetSkill(SkillDefOf.Construction) == null ||
                        pawn.skills.GetSkill(SkillDefOf.Construction).TotallyDisabled
                            ? pawn.thingIDNumber
                            : maxSkillLevel - pawn.skills.GetSkill(SkillDefOf.Construction).Level;
                    break;
                case SortChoice.Cooking:
                    DisplayOrderGetter = pawn =>
                        pawn.skills?.GetSkill(SkillDefOf.Cooking) == null ||
                        pawn.skills.GetSkill(SkillDefOf.Cooking).TotallyDisabled
                            ? _noSkill
                            : -pawn.skills.GetSkill(SkillDefOf.Cooking).Level;
                    NextThingIDNumberGetter = pawn =>
                        pawn.skills?.GetSkill(SkillDefOf.Cooking) == null ||
                        pawn.skills.GetSkill(SkillDefOf.Cooking).TotallyDisabled
                            ? pawn.thingIDNumber
                            : maxSkillLevel - pawn.skills.GetSkill(SkillDefOf.Cooking).Level;
                    break;
                case SortChoice.Mining:
                    DisplayOrderGetter = pawn =>
                        pawn.skills?.GetSkill(SkillDefOf.Mining) == null ||
                        pawn.skills.GetSkill(SkillDefOf.Mining).TotallyDisabled
                            ? _noSkill
                            : -pawn.skills.GetSkill(SkillDefOf.Mining).Level;
                    NextThingIDNumberGetter = pawn =>
                        pawn.skills?.GetSkill(SkillDefOf.Mining) == null ||
                        pawn.skills.GetSkill(SkillDefOf.Mining).TotallyDisabled
                            ? pawn.thingIDNumber
                            : maxSkillLevel - pawn.skills.GetSkill(SkillDefOf.Mining).Level;
                    break;
                case SortChoice.Intellectual:
                    DisplayOrderGetter = pawn =>
                        pawn.skills?.GetSkill(SkillDefOf.Intellectual) == null ||
                        pawn.skills.GetSkill(SkillDefOf.Intellectual).TotallyDisabled
                            ? _noSkill
                            : -pawn.skills.GetSkill(SkillDefOf.Intellectual).Level;
                    NextThingIDNumberGetter = pawn =>
                        pawn.skills?.GetSkill(SkillDefOf.Intellectual) == null ||
                        pawn.skills.GetSkill(SkillDefOf.Intellectual).TotallyDisabled
                            ? _noSkill
                            : maxSkillLevel - pawn.skills.GetSkill(SkillDefOf.Intellectual).Level;
                    break;
                case SortChoice.Plants:
                    DisplayOrderGetter = pawn =>
                        pawn.skills?.GetSkill(SkillDefOf.Plants) == null ||
                        pawn.skills.GetSkill(SkillDefOf.Plants).TotallyDisabled
                            ? _noSkill
                            : -pawn.skills.GetSkill(SkillDefOf.Plants).Level;
                    NextThingIDNumberGetter = pawn =>
                        pawn.skills?.GetSkill(SkillDefOf.Plants) == null ||
                        pawn.skills.GetSkill(SkillDefOf.Plants).TotallyDisabled
                            ? pawn.thingIDNumber
                            : 20 - pawn.skills.GetSkill(SkillDefOf.Plants).Level;
                    break;
                case SortChoice.Animals:
                    DisplayOrderGetter = pawn =>
                        pawn.skills?.GetSkill(SkillDefOf.Animals) == null ||
                        pawn.skills.GetSkill(SkillDefOf.Animals).TotallyDisabled
                            ? _noSkill
                            : -pawn.skills.GetSkill(SkillDefOf.Animals).Level;
                    NextThingIDNumberGetter = pawn =>
                        pawn.skills?.GetSkill(SkillDefOf.Animals) == null ||
                        pawn.skills.GetSkill(SkillDefOf.Animals).TotallyDisabled
                            ? pawn.thingIDNumber
                            : 20 - pawn.skills.GetSkill(SkillDefOf.Animals).Level;
                    break;
                case SortChoice.Melee:
                    DisplayOrderGetter = pawn =>
                        pawn.skills?.GetSkill(SkillDefOf.Melee) == null ||
                        pawn.skills.GetSkill(SkillDefOf.Melee).TotallyDisabled
                            ? _noSkill
                            : -pawn.skills.GetSkill(SkillDefOf.Melee).Level;
                    NextThingIDNumberGetter = pawn =>
                        pawn.skills?.GetSkill(SkillDefOf.Melee) == null ||
                        pawn.skills.GetSkill(SkillDefOf.Melee).TotallyDisabled
                            ? pawn.thingIDNumber
                            : 20 - pawn.skills.GetSkill(SkillDefOf.Melee).Level;
                    break;
                case SortChoice.Art:
                    DisplayOrderGetter = pawn =>
                        pawn.skills?.GetSkill(SkillDefOf.Artistic) == null ||
                        pawn.skills.GetSkill(SkillDefOf.Artistic).TotallyDisabled
                            ? _noSkill
                            : -pawn.skills.GetSkill(SkillDefOf.Artistic).Level;
                    NextThingIDNumberGetter = pawn =>
                        pawn.skills?.GetSkill(SkillDefOf.Artistic) == null ||
                        pawn.skills.GetSkill(SkillDefOf.Artistic).TotallyDisabled
                            ? pawn.thingIDNumber
                            : 20 - pawn.skills.GetSkill(SkillDefOf.Artistic).Level;
                    break;
                case SortChoice.Sleep:
                    DisplayOrderGetter = pawn =>
                        pawn.needs?.rest?.CurLevelPercentage == null
                            ? _noSkill
                            : -(int)(pawn.needs.rest.CurLevelPercentage * 100);
                    NextThingIDNumberGetter = pawn =>
                        pawn.needs?.rest?.CurLevelPercentage == null
                            ? pawn.thingIDNumber
                            : (int)((1f - pawn.needs.rest.CurLevelPercentage) * 100);
                    break;
                case SortChoice.Food:
                    DisplayOrderGetter = pawn =>
                        pawn.needs?.food?.CurLevelPercentage == null
                            ? _noSkill
                            : -(int)(pawn.needs.food.CurLevelPercentage * 100);
                    NextThingIDNumberGetter = pawn =>
                        pawn.needs?.food?.CurLevelPercentage == null
                            ? pawn.thingIDNumber
                            : (int)((1f - pawn.needs.food.CurLevelPercentage) * 100);
                    break;
                case SortChoice.Recreation:
                    DisplayOrderGetter = pawn =>
                        pawn.needs?.joy?.CurLevelPercentage == null
                            ? _noSkill
                            : -(int)(pawn.needs.joy.CurLevelPercentage * 100);
                    NextThingIDNumberGetter = pawn =>
                        pawn.needs?.joy?.CurLevelPercentage == null
                            ? pawn.thingIDNumber
                            : (int)((1f - pawn.needs.joy.CurLevelPercentage) * 100);
                    break;
                case SortChoice.Mood:
                    DisplayOrderGetter = pawn =>
                        pawn.needs?.mood?.CurLevelPercentage == null
                            ? _noSkill
                            : -(int)(pawn.needs.mood.CurLevelPercentage * 100);
                    NextThingIDNumberGetter = pawn =>
                        pawn.needs?.mood?.CurLevelPercentage == null
                            ? pawn.thingIDNumber
                            : (int)((1f - pawn.needs.mood.CurLevelPercentage) * 100);
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