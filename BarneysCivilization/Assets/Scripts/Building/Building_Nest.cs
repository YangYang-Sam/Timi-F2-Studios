using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Nest : Building_Base
{
    public int ResourceAmount = 1;

    protected override void OnGameStateChange()
    {
        base.OnGameStateChange();

        if (InGameManager.isGameState(GameStateType.BeforeMove))
        {
            if (Cell.GetUnitOnCell() && Cell.GetUnitOnCell().Owner == Owner)
            {
                Cell.GetUnitOnCell().canMove = false;
            }
        }

        if (InGameManager.isGameState(GameStateType.AfterBattle))
        {
            if (Cell.GetUnitOnCell() && Cell.GetUnitOnCell().Owner == Owner)
            {
                Cell.GetUnitOnCell().ChangeHealth(ResourceAmount, Cell.transform.position);
            }
        }
    }
}
