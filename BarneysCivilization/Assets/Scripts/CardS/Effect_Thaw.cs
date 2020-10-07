using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Thaw : CardEffect
{
    public int CenterHealthReduceAmount = 1;
    public int NearbyHealthAddAmount = 1;
    public int MinSupportLevel = 2;

    public bool IsSupportCell(CardManager user, HexCell cell)
    {
        if(cell && cell.OwnerManager == user && cell.GetUnitOnCell())
        {
            return cell.GetUnitOnCell().Level >= MinSupportLevel;
        }

        return false;
    }

    public override bool CanUseCard(CardManager user, HexCell cell)
    {
        return base.CanUseCard(user, cell) && IsSupportCell(user, cell);
    }
    public override UseCardFailReason GetFailReason(CardManager user, HexCell cell)
    {
        if (cell && cell.OwnerManager == user && cell.GetUnitOnCell() && cell.GetUnitOnCell().Level < MinSupportLevel)
        {
            return UseCardFailReason.InvalidUnitLevel;
        }
        return base.GetFailReason(user, cell);
    }

    public override List<HexCell> GetCanUseCells(CardManager user)
    {
        List<HexCell> cells = new List<HexCell>();
        foreach (var cell in user.OccupiedCells)
        {
            if (IsSupportCell(user, cell))
            {
                cells.Add(cell);
            }
        }
        return cells;
    }

    public override void Effect(CardManager user, HexCell cell)
    {
        if(cell)
        {
            int ReduceAmount = 0;
            foreach(var nearbyCell in cell.NearbyCells)
            {
                if(nearbyCell && (nearbyCell.OwnerManager == user) && nearbyCell.GetUnitOnCell())
                {
                    nearbyCell.GetUnitOnCell().ChangeHealth(NearbyHealthAddAmount,cell.transform.position);
                    ReduceAmount += CenterHealthReduceAmount;
                }
            }

            if(cell && cell.GetUnitOnCell())
            {
                int maxReduceAmount = cell.GetUnitOnCell().GetTotalHealth() > 0 ? cell.GetUnitOnCell().GetTotalHealth() - 1 : 0; 
                ReduceAmount = Mathf.Clamp(ReduceAmount, 0, maxReduceAmount);
                cell.GetUnitOnCell().TakeDamage(ReduceAmount, null);
            }
        }
    }
}
