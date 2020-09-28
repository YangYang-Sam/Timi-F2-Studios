using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_CellProduce : CardEffect
{
    public List<HexCellType> cellType;
    public int amount;
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
        foreach(HexCell c in user.OccupiedCells)
        {
            if (cellType.Contains(c.CellType))
            {
                c.GetUnitOnCell().ChangeHealth(amount);
            }
        }
    }
}
