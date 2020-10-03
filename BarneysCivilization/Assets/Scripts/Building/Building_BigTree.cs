using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_BigTree : Building_Base 
{
    public override void OnCreated(HexCell cell, CardManager owner)
    {
        base.OnCreated(cell, owner);
        cell.MinUnitAmount += 9;
    }
    public override void OnBuildingDestroy()
    {
        base.OnBuildingDestroy();
        Cell.MinUnitAmount -= 9;
    }
    protected override void OnGameStateChange()
    {
        base.OnGameStateChange();
        if (InGameManager.isGameState(GameStateType.AfterBattle))
        {
            Cell.GetUnitOnCell().ChangeHealth(1,transform.position);
            if (Cell.GetUnitOnCell().Level == 3)
            {
                OnBuildingDestroy();
            }
        }
    }
}
