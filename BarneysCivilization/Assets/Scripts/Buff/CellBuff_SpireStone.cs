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
            for (int i = Cell.PlacedUnits.Count-1; i >=0 ; i--)
            {
                if (Cell.PlacedUnits[i].Owner != Creator)
                {
                    Cell.PlacedUnits[i].TakeDamage(HealthReduceAmount, null);
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
