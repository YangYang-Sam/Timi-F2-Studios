using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuff_Superconduct : PlayerBuff_Base
{
    public int SpeedAddAmount = 2;
    public int HealthAddApmount = 2;

    protected override void OnUnitBeforeBattle(Unit_Base unit, HexCell cell)
    {
        base.OnUnitBeforeBattle(unit, cell);

        unit.ChangeHealth(HealthAddApmount, Creator.GetCorePosition());
    }

    protected override void OnGameStateChange()
    {
        base.OnGameStateChange();

        if (InGameManager.isGameState(GameStateType.BeforeMove))
        {
            Creator.UnitMoveSpeed += SpeedAddAmount;
        }
    }

}
