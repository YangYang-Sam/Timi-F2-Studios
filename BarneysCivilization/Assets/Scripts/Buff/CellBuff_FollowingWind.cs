using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBuff_FollowingWind : CellBuff_Base
{
    public override void OnCreated(HexCell cell, CardManager creator)
    {
        base.OnCreated(cell, creator);
        UpdateBuffType(CellBuffType.FollowingWind);
        InGameManager.instance.GameStateChangeEvent += OnGameStateChangeInFollowingWindBuff;
    }

    private void OnGameStateChangeInFollowingWindBuff()
    {
        if (InGameManager.isGameState(GameStateType.BeforeMove))
        {
            if (Cell.GetUnitOnCell() && (Cell.GetUnitOnCell().Owner == Creator))
            {
                Cell.GetUnitOnCell().canMove = true;
            }
        }
    }

    public override void OnBuffDestroy()
    {
        base.OnBuffDestroy();
        InGameManager.instance.GameStateChangeEvent -= OnGameStateChangeInFollowingWindBuff;
    }
}
