using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Tinder : Building_Base
{
    public int Amount;

    protected override void OnGameStateChange()
    {
        base.OnGameStateChange();

        if (InGameManager.isGameState(GameStateType.Decision))
        {
            List<HexCell> cells = new List<HexCell>();
            cells.Add(Cell);
            foreach (var nearbyCell in Cell.NearbyCells)
            {
                if(nearbyCell.OwnerManager == Owner)
                {
                    cells.Add(nearbyCell);
                }          
            }

            foreach (var cell in cells)
            {
                foreach (var buff in cell.CellBuffs)
                {
                    if(buff.BuffType == CellBuffType.Fire)
                    {
                        cell.GetUnitOnCell().ChangeHealth(Amount);
                        break;
                    }
                }
            }
        }
    }
}
