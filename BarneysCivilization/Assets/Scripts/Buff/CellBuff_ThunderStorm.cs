using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBuff_ThunderStorm : CellBuff_Base
{
    public int DamageHealthAmount = 1;

    public override void OnCreated(HexCell cell, CardManager creator)
    {
        base.OnCreated(cell, creator);
        UpdateBuffType(CellBuffType.ThunderStorm);
        InGameManager.instance.GameStateChangeEvent += OnGameStateChangeInThunderStormBuff;
    }

    private void OnGameStateChangeInThunderStormBuff()
    {
        if (InGameManager.isGameState(GameStateType.AfterBattle))
        {
            foreach (var nearbyCell in Cell.NearbyCells)
            {
                if(nearbyCell.GetUnitOnCell())
                {
                    nearbyCell.GetUnitOnCell().TakeDamage(DamageHealthAmount, null);
                }
            }
        }
    }

    public override void OnBuffDestroy()
    {
        base.OnBuffDestroy();
        InGameManager.instance.GameStateChangeEvent -= OnGameStateChangeInThunderStormBuff;
    }
}
