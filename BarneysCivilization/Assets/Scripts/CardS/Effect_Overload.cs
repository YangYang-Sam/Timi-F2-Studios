using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Overload : CardEffect
{
    public int MinLevel=2;
    public int ResourceCost;
    public int HealthAmount;

    public override bool CanUseCard(CardManager user, HexCell cell)
    {
        return base.CanUseCard(user, cell) && cell.GetUnitOnCell() && (cell.GetUnitOnCell().Level >= MinLevel && user.GetTotalResource() >= 0);
    }
    public override UseCardFailReason GetFailReason(CardManager user, HexCell cell)
    {
        if (cell.GetUnitOnCell() && (cell.GetUnitOnCell().Level < MinLevel))
        {
            return UseCardFailReason.InvalidUnitLevel;
        }
        return base.GetFailReason(user, cell);
    }
    public override List<HexCell> GetCanUseCells(CardManager user)
    {
        List<HexCell> cells = new List<HexCell>();
        foreach (var cell in user.GetAllNearbyCells())
        {
            if(cell.GetUnitOnCell() && (cell.GetUnitOnCell().Level >= MinLevel))
            {
                cells.Add(cell);
            }
        }       
        return cells;
    }

    public override void Effect(CardManager user, HexCell cell)
    {
        if(cell.GetUnitOnCell())
        {
            cell.GetUnitOnCell().ChangeHealth(HealthAmount, user.GetCorePosition());
        }
        
        ArtResourceManager.instance.CreateTextEffect("过载", cell.transform.position);
    }
}
