using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Combustion : CardEffect
{
    public int HealthAddAmount = 1;
    public int EffectIndex = 5;
    public bool HasFireBuff(HexCell cell)
    {
        foreach(var buff in cell.CellBuffs)
        {
            if(buff.BuffType == CellBuffType.Fire)
            {
                return true;
            }
        }
        return false;
    }

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
        foreach(var occupiedCell in user.OccupiedCells)
        {
            if(HasFireBuff(occupiedCell))
            {
                occupiedCell.GetUnitOnCell().ChangeHealth(HealthAddAmount, user.GetCorePosition());
            }
            ArtResourceManager.instance.CreateEffectByIndex(occupiedCell.transform.position, EffectIndex);
            ArtResourceManager.instance.CreateTextEffect("助燃", occupiedCell.transform.position);
        }
    }
}
