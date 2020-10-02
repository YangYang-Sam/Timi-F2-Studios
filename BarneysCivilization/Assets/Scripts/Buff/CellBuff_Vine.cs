using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBuff_Vine : CellBuff_Base
{
    public override void OnCreated(HexCell cell, CardManager creator)
    {
        base.OnCreated(cell, creator);
        UpdateBuffType(CellBuffType.Vine);
        InGameManager.instance.GameStateChangeEvent += OnGameStateChangeInVineBuff;
    }

    private void OnGameStateChangeInVineBuff()
    {
        if (InGameManager.isGameState(GameStateType.BeforeMove))
        {
            foreach (var nearbyCell in Cell.NearbyCells)
            {
                if(nearbyCell && (nearbyCell.OwnerManager != Creator) && nearbyCell.GetUnitOnCell())
                {
                    nearbyCell.GetUnitOnCell().canMove = false;
                }
            }

            if(Cell && (Cell.OwnerManager != Creator) && Cell.GetUnitOnCell())
            {
                Cell.GetUnitOnCell().canMove = false;
            }
        }
    }

    public override void OnBuffDestroy()
    {
        base.OnBuffDestroy();
        InGameManager.instance.GameStateChangeEvent -= OnGameStateChangeInVineBuff;
    }
}
