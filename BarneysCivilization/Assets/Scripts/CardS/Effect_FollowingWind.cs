using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_FollowingWind : CardEffect
{
    public int ActionPointAddAmount = 1;

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
        user.ActionPoint += ActionPointAddAmount;

        foreach(var unit in user.Units)
        {
            unit.ForceMove = true;
        }
    }
}
