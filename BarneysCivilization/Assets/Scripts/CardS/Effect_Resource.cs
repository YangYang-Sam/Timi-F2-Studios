using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Resource : CardEffect
{
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
        Unit_Base unit = cell.GetUnitOnCell();
        unit.ChangeHealth(user.GetTotalResource());
    }
}