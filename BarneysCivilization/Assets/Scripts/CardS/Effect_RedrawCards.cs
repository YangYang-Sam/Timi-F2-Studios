using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_RedrawCards : CardEffect
{
    public int ActionPoint;
    public int DrawCards;
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
        user.AbandonAllCards();
        if (DrawCards > 0)
        {
            user.DrawCards(DrawCards);
        }
        user.ActionPoint = ActionPoint+1;
    }
}
