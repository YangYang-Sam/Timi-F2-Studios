using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBuff_IceStorm : CellBuff_Base
{
    public int DamageHealthAmount = 2;

    public override void OnCreated(HexCell cell, CardManager creator)
    {
        base.OnCreated(cell, creator);
        UpdateBuffType(CellBuffType.IceStorm);
        InGameManager.instance.GameStateChangeEvent += OnGameStateChangeInIceStormBuff;
    }

    private void OnGameStateChangeInIceStormBuff()
    {
        if (InGameManager.isGameState(GameStateType.BeforeMove))
        {
            foreach (var nearbyCell in Cell.NearbyCells)
            {
                if (nearbyCell.GetUnitOnCell())
                {
                    nearbyCell.GetUnitOnCell().TakeDamage(DamageHealthAmount, null);
                }
            }
        }
    }

    public override void OnBuffDestroy()
    {
        base.OnBuffDestroy();
        InGameManager.instance.GameStateChangeEvent -= OnGameStateChangeInIceStormBuff;
    }
}
