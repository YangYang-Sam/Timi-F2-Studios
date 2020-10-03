using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_HotWater : CardEffect
{
    public int HealthAmount = 1;
    public int MinLevel = 2;

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
            var unit = occupiedCell.GetUnitOnCell();
            if(unit.Level >= MinLevel)
            {
                unit.ChangeHealth(HealthAmount, user.GetCorePosition());
            }           
       }
    }
}
