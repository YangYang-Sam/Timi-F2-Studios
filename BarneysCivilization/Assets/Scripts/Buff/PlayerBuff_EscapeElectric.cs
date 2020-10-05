using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuff_EscapeElectric : PlayerBuff_Base
{   
    private void OnDeathEvent(int damage, Unit_Base unit)
    {
        foreach(var nearbyCell in unit.Cell.NearbyCells)
        {
            if (nearbyCell)
            {
                foreach (var otherUnit in nearbyCell.PlacedUnits)
                {
                    if (otherUnit && (otherUnit.Owner == Creator))
                    {
                        unit.ChangeHealth(damage / 2,unit.Cell.transform.position);
                        return;
                    }
                }
            }
        }
    }

    protected override void OnUnitBeforeBattle(Unit_Base unit, HexCell cell)
    {
        base.OnUnitBeforeBattle(unit, cell);
        unit.DeathEvent += OnDeathEvent;
    }
}
