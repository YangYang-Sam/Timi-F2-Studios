using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_TempResourceByCellType : CardEffect
{
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
        var occupiedCells = user.OccupiedCells;
        foreach(var occupiedCell in occupiedCells)
        {
            if(occupiedCell.CellType == CellType)
            {
                user.TempResourceAmount++;
            }
        }
    }

    public HexCellType CellType;
}
