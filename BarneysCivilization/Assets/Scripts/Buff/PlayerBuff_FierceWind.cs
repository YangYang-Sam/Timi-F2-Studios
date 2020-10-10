using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuff_FierceWind : PlayerBuff_Base
{
    protected override void OnUnitBeforeBattle(Unit_Base unit, HexCell cell)
    {
        base.OnUnitBeforeBattle(unit, cell);

        int DamageAmount =3/* unit.Level*/;

        if (cell.PlacedUnits.Count > 0)
        {
            ArtResourceManager.instance.CreateTextEffect("狂风", cell.transform.position);
            ArtResourceManager.instance.CreateEffectByIndex(cell.transform.position, 21);
        }
        foreach (var placedUnit in cell.PlacedUnits)
        {
            if(placedUnit != unit)
            {
                placedUnit.TakeDamage(DamageAmount, null);
            }         
        }
    }
}
