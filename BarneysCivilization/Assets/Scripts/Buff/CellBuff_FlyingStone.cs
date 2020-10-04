using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBuff_FlyingStone : CellBuff_Base
{
    public override void OnCreated(HexCell cell, CardManager creator)
    {
        base.OnCreated(cell, creator);
        UpdateBuffType(CellBuffType.FlyingStone);
        InGameManager.instance.GameStateChangeEvent += OnGameStateChangeInFlyingStoneBuff;
    }

    private void OnGameStateChangeInFlyingStoneBuff()
    {
        if (InGameManager.isGameState(GameStateType.BeforeMove))
        {
            int Damage = 0;

            foreach (var cell in Cell.NearbyCells)
            {
                if (cell.CellType == HexCellType.Desert || cell.CellType == HexCellType.Hill || cell.CanPass == false)
                {
                    Damage++;
                }
            }

            if (Cell.GetUnitOnCell())
            {
                Cell.GetUnitOnCell().TakeDamage(Damage, null);
            }
        }
    }

    public override void OnBuffDestroy()
    {
        base.OnBuffDestroy();
        InGameManager.instance.GameStateChangeEvent -= OnGameStateChangeInFlyingStoneBuff;
    }
}
