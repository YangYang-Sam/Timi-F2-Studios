using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Magnet : Building_Base
{
    protected override void OnGameStateChange()
    {
        base.OnGameStateChange();

        if (InGameManager.isGameState(GameStateType.BeforeMove))
        {
            foreach(var neabyCell in Cell.NearbyCells)
            {
                if(neabyCell.GetUnitOnCell() && (neabyCell.GetUnitOnCell().Owner != Owner))
                {
                    neabyCell.GetUnitOnCell().TempPathCells.Clear();
                    neabyCell.GetUnitOnCell().TempPathCells.Add(Cell);
                }
            }
        }
    }
}
