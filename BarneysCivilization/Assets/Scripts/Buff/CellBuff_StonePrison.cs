using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBuff_StonePrison : CellBuff_Base
{
    public int LockedTurns = 1;

    public override void OnCreated(HexCell cell, CardManager creator)
    {
        base.OnCreated(cell, creator);
        UpdateBuffType(CellBuffType.StonePrison);
        InGameManager.instance.GameStateChangeEvent += OnGameStateChangeInStonePrisonBuff;
    }

    private void OnGameStateChangeInStonePrisonBuff()
    {
        if (InGameManager.isGameState(GameStateType.BeforeMove) && (Turns <= LockedTurns))
        {
            Cell.CanPass = false;

            if(Cell.GetUnitOnCell())
            {
                Cell.GetUnitOnCell().CanMove = false;
            }
        }
    }

    public override void OnBuffDestroy()
    {
        base.OnBuffDestroy();
        InGameManager.instance.GameStateChangeEvent -= OnGameStateChangeInStonePrisonBuff;
        Cell.CanPass = true;
    }
}
