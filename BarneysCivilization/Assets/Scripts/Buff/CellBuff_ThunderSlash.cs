using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBuff_ThunderSlash : CellBuff_Base
{
    public int EnemyReduceAmount = 3;
    public int MyReduceAmount = 1;


    public override void OnCreated(HexCell cell, CardManager creator)
    {
        base.OnCreated(cell, creator);
        UpdateBuffType(CellBuffType.ThunderSlash);
        InGameManager.instance.GameStateChangeEvent += OnGameStateChangeInThunderSlashBuff;
    }

    private void OnGameStateChangeInThunderSlashBuff()
    {
        if (InGameManager.isGameState(GameStateType.BeforeMove))
        {
            int EnemyReduceTotalAmount = 0;
            foreach (var nearbyCell in Cell.NearbyCells)
            {
                if (nearbyCell && (nearbyCell.OwnerManager == Creator) && nearbyCell.GetUnitOnCell())
                {
                    nearbyCell.GetUnitOnCell().TakeDamage(MyReduceAmount, null);
                    EnemyReduceTotalAmount += EnemyReduceAmount;
                }
            }

            if (Cell && (Cell.OwnerManager != Creator) && Cell.GetUnitOnCell())
            {
                Cell.GetUnitOnCell().TakeDamage(EnemyReduceTotalAmount, null);
            }
        }
    }

    public override void OnBuffDestroy()
    {
        base.OnBuffDestroy();
        InGameManager.instance.GameStateChangeEvent -= OnGameStateChangeInThunderSlashBuff;
    }
}
