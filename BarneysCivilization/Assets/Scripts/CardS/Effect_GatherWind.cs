using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_GatherWind : CardEffect
{
    public override bool CanUseCard(CardManager user, HexCell cell)
    {
        return base.CanUseCard(user, cell);
    }

    public override List<HexCell> GetCanUseCells(CardManager user)
    {
        return base.GetCanUseCells(user);
    }

    public override void Effect(CardManager user, HexCell cell)
    {
        user.SetUnitMoveModeTemporarily(UnitMoveMode.JumpThreeStep);
        ArtResourceManager.instance.CreateTextEffect("汇聚之风", user.GetCorePosition());
        ArtResourceManager.instance.CreateEffectByIndex(user.GetCorePosition(), 18);
    }
}
