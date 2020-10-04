using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_SandstormTower : Building_Base
{
    public int ActionPointAmount = 1;

    protected override void OnGameStateChange()
    {
        base.OnGameStateChange();

        if (InGameManager.isGameState(GameStateType.BeforeMove))
        {
            bool hasUserNearbyCell = false;

            foreach(var nearbyCell in Cell.NearbyCells)
            {
                if(nearbyCell.OwnerManager == Owner)
                {
                    hasUserNearbyCell = true;
                    break;
                }
            }

            if(!hasUserNearbyCell)
            {
                Owner.ActionPoint += ActionPointAmount;
            }
        }
    }
}
