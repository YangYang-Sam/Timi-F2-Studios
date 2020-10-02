using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBuff_RainStorm : CellBuff_Base
{
    public override void OnCreated(HexCell cell, CardManager creator)
    {
        base.OnCreated(cell, creator);
        UpdateBuffType(CellBuffType.RainStorm);
        InGameManager.instance.GameStateChangeEvent += OnGameStateChangeInRainStormBuff;
    }

    private void OnGameStateChangeInRainStormBuff()
    {
        if (InGameManager.isGameState(GameStateType.BeforeMove) && Cell.GetUnitOnCell())
        {
            int Damage = Cell.GetUnitOnCell().Level;
            Cell.GetUnitOnCell().TakeDamage(Damage, null);
        }
    }

    public override void OnBuffDestroy()
    {
        base.OnBuffDestroy();
        InGameManager.instance.GameStateChangeEvent -= OnGameStateChangeInRainStormBuff;
    }
}
