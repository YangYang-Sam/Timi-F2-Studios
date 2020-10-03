using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_AreaAttack : CardEffect
{
    public int AllyAmount;
    public int EnemyAmount;
    public override bool CanUseCard(CardManager user, HexCell cell)
    {
        return base.CanUseCard(user, cell);
    }
    public override List<HexCell> GetCanUseCells(CardManager user)
    {
        return user.GetAllNearbyCells();
    }
    public override void Effect(CardManager user, HexCell cell)
    {
        List<HexCell> CheckList = new List<HexCell>();
        CheckList.Add(cell);
        foreach(HexCell neighbor in cell.NearbyCells)
        {
            CheckList.Add(neighbor);
        }

        foreach(HexCell checkCell in CheckList)
        {
            if (checkCell.OwnerManager == user)
            {
                checkCell.GetUnitOnCell().AddTempHealth(AllyAmount);
            }
            else if(checkCell.OwnerManager!=null)
            {
                checkCell.GetUnitOnCell().AddTempHealth(-EnemyAmount);
            }
        }

    }
}
