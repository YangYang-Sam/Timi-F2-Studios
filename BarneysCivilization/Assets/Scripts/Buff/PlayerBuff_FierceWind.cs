using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuff_FierceWind : PlayerBuff_Base
{
    protected override void OnUnitBeforeBattle(Unit_Base unit, HexCell cell)
    {
        base.OnUnitBeforeBattle(unit, cell);

        int DamageAmount = unit.Level;

        foreach (var placedUnit in cell.PlacedUnits)
        {
            if(placedUnit != unit)
            {
                placedUnit.TakeDamage(DamageAmount, null);
            }         
        }
    }
}
