using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Mine : Building_Base
{
    public int ResourceAmount;

    protected override void OnGameStateChange()
    {
        base.OnGameStateChange();

        if (InGameManager.isGameState(GameStateType.AfterBattle))
        {
            Owner.TempResourceAmount += ResourceAmount;
        }
    }
}
