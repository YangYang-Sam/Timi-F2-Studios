using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuff_Flood : PlayerBuff_Base
{
    protected override void OnGameStateChange()
    {
        base.OnGameStateChange();
        if (InGameManager.isGameState(GameStateType.BeforeBattle))
        {
            foreach(Unit_Base unit in Owner.Units)
            {
                if (unit.Level > 1)
                {
                    for (int i = unit.Cell.CellBuffs.Count - 1; i >= 0; i--)
                    {
                        if (unit.Cell.CellBuffs[i].Creator != Owner)
                        {
                            unit.Cell.CellBuffs[i].OnBuffDestroy();
                        }
                    }
                    if(unit.Cell.OwnerManager!=Owner && unit.Cell.PlacedBuilding != null)
                    {
                        unit.Cell.PlacedBuilding.OnBuildingDestroy();
                    }
                }
            }
        }
    }
}
