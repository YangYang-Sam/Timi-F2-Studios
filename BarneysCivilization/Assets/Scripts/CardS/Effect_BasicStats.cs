using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_BasicStats : CardEffect
{
    public int TempResource;
    public int ActionPoint;
    public int DrawCards;
    public int MoveSpeed;
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
        user.ActionPoint += ActionPoint;
        user.UnitMoveSpeed += MoveSpeed;
        user.TempResourceAmount += TempResource;
        if (DrawCards > 0)
        {
            user.DrawCards(DrawCards);
        }
    }
}
