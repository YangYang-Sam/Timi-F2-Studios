using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_StunEnemy : CardEffect
{
    public int EffectIndex;
    public float EffectDuration=1;
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
        List<HexCell> CheckList = new List<HexCell>();
        CheckList.Add(cell);
        foreach (HexCell neighbor in cell.NearbyCells)
        {
            CheckList.Add(neighbor);
        }

        foreach (HexCell checkCell in CheckList)
        {
            if (checkCell.OwnerManager != user && checkCell.OwnerManager!=null && checkCell.GetUnitOnCell())
            {
                checkCell.GetUnitOnCell().canMove = false;
            }
        }
        ArtResourceManager.instance.CreateEffectByIndex(cell.transform.position, EffectIndex, EffectDuration);
    }
}
