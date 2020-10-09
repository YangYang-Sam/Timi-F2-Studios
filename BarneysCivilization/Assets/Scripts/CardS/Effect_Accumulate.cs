using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Accumulate : CardEffect
{
    public int HealthAddAmount = 2;
    public int MaxLevel = 1;

    public override bool CanUseCard(CardManager user, HexCell cell)
    {
        return base.CanUseCard(user, cell) && cell.GetUnitOnCell() && (cell.GetUnitOnCell().Level <= MaxLevel);
    }

    public override List<HexCell> GetCanUseCells(CardManager user)
    {
        return user.OccupiedCells;
    }
    public override UseCardFailReason GetFailReason(CardManager user, HexCell cell)
    {
        if (cell.GetUnitOnCell() && (cell.GetUnitOnCell().Level > MaxLevel))
        {
            return UseCardFailReason.InvalidUnitLevel;
        }
        return base.GetFailReason(user, cell);
    }
    public override void Effect(CardManager user, HexCell cell)
    {
        if(cell.GetUnitOnCell())
        {
            cell.GetUnitOnCell().ChangeHealth(HealthAddAmount, cell.transform.position);
            cell.GetUnitOnCell().CanMove = false;
        }
    }
}
