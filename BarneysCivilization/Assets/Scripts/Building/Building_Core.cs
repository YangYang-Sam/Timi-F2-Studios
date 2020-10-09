using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Core : Building_Base
{
    protected override void OnGameStateChange()
    {
        base.OnGameStateChange();
        if (InGameManager.isGameState(GameStateType.Decision) && Cell.GetUnitOnCell())
        {
            Cell.GetUnitOnCell().ChangeHealth(Owner.GetTotalResource(),Owner.GetCorePosition());
        }
    }
}
