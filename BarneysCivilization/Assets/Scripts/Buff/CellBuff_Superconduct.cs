using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBuff_Superconduct : CellBuff_Base
{
    public int SpeedAmount;
    public int EffectIndex = 8;
    public override void OnCreated(HexCell cell, CardManager creator)
    {
        base.OnCreated(cell, creator);
        UpdateBuffType(CellBuffType.SuperConduct);
        cell.CellBeforeBattleEvent += OnCellBeforeBattle;
        Creator.UnitMoveSpeed += SpeedAmount;
    }

    private void OnCellBeforeBattle(HexCell obj)
    {
        foreach(Unit_Base unit in Cell.PlacedUnits)
        {
            if (unit.Owner == Creator)
            {
                if (unit)
                {
                    unit.ChangeHealth(2, transform.position);
                }              
                ArtResourceManager.instance.CreateTextEffect("超导", transform.position);
            }
        }
    }

    public override void OnBuffDestroy()
    {
        base.OnBuffDestroy();
        Cell.CellBeforeBattleEvent -= OnCellBeforeBattle;
    }
}
