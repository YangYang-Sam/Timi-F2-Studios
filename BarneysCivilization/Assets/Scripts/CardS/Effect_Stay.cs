using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Stay : CardEffect
{
    public int amount;
    public override bool CanUseCard(CardManager user, HexCell cell)
    {
        return base.CanUseCard(user, cell);
    }
    public override List<HexCell> GetCanUseCells(CardManager user)
    {
        return user.OccupiedCells;
    }
    public override void Effect(CardManager user, HexCell cell)
    {
        foreach(HexCell c in user.OccupiedCells)
        {
            if(c.CellType== HexCellType.Hill)
            {
                cell.GetUnitOnCell().ChangeHealth(amount, c.transform.position);
            }
        }
        cell.GetUnitOnCell().canMove = false;    
    }
}
