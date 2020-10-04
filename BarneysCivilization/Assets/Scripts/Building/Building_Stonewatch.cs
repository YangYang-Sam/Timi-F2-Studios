using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Stonewatch : Building_Base
{
    public int ReserveAmount = 4;

    protected override void OnGameStateChange()
    {
        base.OnGameStateChange();

        if (InGameManager.isGameState(GameStateType.BeforeMove))
        {
            int ReserveHealth = ReserveAmount;

            foreach (var building in Owner.Buildings)
            {
                if (building.GetComponent<Building_Stonewatch>())
                {
                    ReserveHealth++;
                }
            }

            if (Cell.GetUnitOnCell() && (Cell.GetUnitOnCell().GetTotalHealth() < ReserveHealth))
            {
                Cell.GetUnitOnCell().canMove = false;
            }
        }
    }
}
