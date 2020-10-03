using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_UnitStats : CardEffect
{
    public int TempHealth;
    public int Health;
    public int ActionPoint;
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
        user.ActionPoint += ActionPoint;
        Unit_Base unit = cell.GetUnitOnCell();
        unit.AddTempHealth(TempHealth);
        unit.ChangeHealth(Health,user.PlayerCore.transform.position);

    }
}