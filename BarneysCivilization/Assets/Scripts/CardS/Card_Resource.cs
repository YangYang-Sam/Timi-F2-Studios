using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Resource : Card_Base
{
    public override bool CanUseCard(CardManager user, HexCell cell)
    {
        return base.CanUseCard(user, cell) && GetCanUseCells(user).Contains(cell);
    }
    public override List<HexCell> GetCanUseCells(CardManager user)
    {
        return user.OccupiedCells;
    }
    public override void CardEffect(CardManager user, HexCell cell)
    {
        base.CardEffect(user, cell);
        cell.PlacedUnits[0].ChangeHealth(user.ResourceAmount + user.TempResourceAmount);
    }
}
