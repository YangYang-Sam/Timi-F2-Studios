using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_River : Card_Base
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
        int amount = 0;
        foreach(HexCell neightbor in user.OccupiedCells)
        {
            if (neightbor.CellType == HexCellType.Water)
            {
                amount++;
            }
        }
        cell.PlacedUnits[0].ChangeHealth(amount);
        cell.PlacedUnits[0].AddTempHealth(3);
    }
}
