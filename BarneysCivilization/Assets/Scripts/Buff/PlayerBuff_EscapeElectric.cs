using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuff_EscapeElectric : PlayerBuff_Base
{   
    private void OnDeathEvent(int damage)
    {
        foreach(var nearbyCell in Cell.NearbyCells)
        {
            if (nearbyCell)
            {
                foreach (var unit in nearbyCell.PlacedUnits)
                {
                    if (unit && (unit.Owner == Creator))
                    {
                        unit.ChangeHealth(damage / 2);
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
