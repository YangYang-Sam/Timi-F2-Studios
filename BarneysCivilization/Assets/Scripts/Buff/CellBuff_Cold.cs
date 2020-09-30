using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBuff_Cold : CellBuff_Base
{
    public override void OnCreated(HexCell cell, CardManager creator)
    {
        base.OnCreated(cell, creator);
        UpdateBuffType(CellBuffType.Cold);
        InGameManager.instance.GameStateChangeEvent += OnGameStateChangeInColdBuff;
    }

    private void OnGameStateChangeInColdBuff()
    {
        if (InGameManager.isGameState(GameStateType.BeforeBattle))
        {
            int Damage = 0;
            foreach(var cell in Creator.OccupiedCells)
            {
                if(cell.CellType == HexCellType.Snow)
                {
                    Damage++;
                }
            }
            
            foreach(var unit in Cell.PlacedUnits)
            {
                if(unit.Owner != Creator)
                {
                    unit.TakeDamage(Damage, null);
                }
            }          
        }
    }

    public override void OnBuffDestroy()
    {
        base.OnBuffDestroy();
        InGameManager.instance.BeforeTurnEndEvent -= OnGameStateChangeInColdBuff;
    }
}
