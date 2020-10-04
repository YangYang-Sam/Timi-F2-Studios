using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_WildernessWind : CardEffect
{
    public int HealthAmount = 1;
    public int ResourceAmount = 1;
    
    public override bool CanUseCard(CardManager user, HexCell cell)
    {
        return base.CanUseCard(user, cell) && (cell.OwnerManager == user);
    }

    public override List<HexCell> GetCanUseCells(CardManager user)
    {
        return user.OccupiedCells;
    }

    public override void Effect(CardManager user, HexCell cell)
    {
        if(cell.GetUnitOnCell() && (cell.GetUnitOnCell().Owner == user))
        {
            cell.GetUnitOnCell().ChangeHealth(HealthAmount, cell.transform.position);

            foreach(var nearbyCell in cell.NearbyCells)
            {
                if((nearbyCell.CellType == HexCellType.Grass) || (nearbyCell.CellType == HexCellType.Desert))
                {
                    if (nearbyCell.OwnerManager != user)
                    {
                        user.TempResourceAmount += ResourceAmount;
                    }
                }
            }
        }
    }
}
