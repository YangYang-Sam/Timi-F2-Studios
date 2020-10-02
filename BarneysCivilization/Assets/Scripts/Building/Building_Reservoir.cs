using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Reservoir : Building_Base
{
    public int ResourceAmount = 1;

    protected override void OnGameStateChange()
    {
        base.OnGameStateChange();

        if (InGameManager.isGameState(GameStateType.AfterBattle))
        {
            foreach (var buff in Cell.CellBuffs)
            { 
                if(buff.BuffType == CellBuffType.Rain)
                {
                    Owner.TempResourceAmount += ResourceAmount;
                    break;
                }
            }        
        }
    }
}
