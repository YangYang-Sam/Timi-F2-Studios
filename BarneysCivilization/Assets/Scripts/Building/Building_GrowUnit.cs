using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_GrowUnit : Building_Base
{
    public int Threshold;
    public int Amount;

    protected override void OnGameStateChange()
    {
        base.OnGameStateChange();
        if (InGameManager.isGameState(GameStateType.Decision))
        {
            if (Cell.GetUnitOnCell().Health <= Threshold)
            {
                Cell.GetUnitOnCell().ChangeHealth(Mathf.Min(Threshold - Cell.GetUnitOnCell().Health, Amount * Level));                
            }          
        }
    }
}
