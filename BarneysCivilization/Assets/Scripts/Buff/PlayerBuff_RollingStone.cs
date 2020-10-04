using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuff_RollingStone : PlayerBuff_Base
{
    public int HealthAddAmount = 1;

    bool IsAcceptedCellType(HexCell cell)
    {
        if(cell.CellType == HexCellType.Hill || cell.CellType == HexCellType.Grass || cell.CellType == HexCellType.Forest)
        {
            return true;
        }

        return false;
    }

    protected override void OnGameStateChange()
    {
        base.OnGameStateChange();

        if (InGameManager.isGameState(GameStateType.BeforeBattle))
        {
            foreach (var unit in Owner.Units)
            {
                if (IsAcceptedCellType(unit.Cell))
                {
                     unit.ChangeHealth(HealthAddAmount, Owner.GetCorePosition());
                }
            }
        }
    }
}
