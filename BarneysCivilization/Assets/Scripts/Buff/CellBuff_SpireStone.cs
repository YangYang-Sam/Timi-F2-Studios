using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBuff_SpireStone : CellBuff_Base
{
    public int HealthReduceAmount = 1;
    
    public override void OnCreated(HexCell cell, CardManager creator)
    {
        base.OnCreated(cell, creator);
        UpdateBuffType(CellBuffType.SpireStone);
        InGameManager.instance.GameStateChangeEvent += OnGameStateChangeInSpireStoneBuff;
    }

    private void OnGameStateChangeInSpireStoneBuff()
    {
        if (InGameManager.isGameState(GameStateType.BeforeBattle))
        {
            foreach (var unit in Cell.PlacedUnits)
            {
                if(unit.Owner != Creator)
                {
                    unit.TakeDamage(HealthReduceAmount, null);
                }
            }
        }
    }

    public override void OnBuffDestroy()
    {
        base.OnBuffDestroy();
        InGameManager.instance.GameStateChangeEvent -= OnGameStateChangeInSpireStoneBuff;
    }
}
