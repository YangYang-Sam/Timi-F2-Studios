﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBuff_SandStorm : CellBuff_Base
{
    public int Damage = 1;

    public override void OnCreated(HexCell cell, CardManager creator)
    {
        base.OnCreated(cell, creator);
        UpdateBuffType(CellBuffType.SandStorm);
        InGameManager.instance.LateDecisionEvent += OnGameStateChangeInColdBuff;
    }
    private void OnGameStateChangeInColdBuff()
    {

        List<Unit_Base> units = new List<Unit_Base>(Cell.PlacedUnits);
        foreach (var nearbyCell in Cell.NearbyCells)
        {
            foreach (var unit in nearbyCell.PlacedUnits)
            {
                units.Add(unit);
            }
        }

        foreach (var unit in units)
        {
            unit.TakeDamage(Damage, null);
        }

    }
    public override void OnBuffDestroy()
    {
        base.OnBuffDestroy();
        InGameManager.instance.LateDecisionEvent -= OnGameStateChangeInColdBuff;
    }
}
