using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Stay : CardEffect
{
    public int amount;
    public override bool CanUseCard(CardManager user, HexCell cell)
    {
        return base.CanUseCard(user, cell) && GetCanUseCells(user).Contains(cell);
    }
    public override List<HexCell> GetCanUseCells(CardManager user)
    {
        return user.OccupiedCells;
    }
    public override void Effect(CardManager user, HexCell cell)
    {
        cell.GetUnitOnCell().canMove = false;
        cell.GetUnitOnCell().ChangeHealth(amount);
    }
}
