using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBuff_BallLightning : CellBuff_Base
{
    public int DamageBuildingAmount = 1;

    public override void OnCreated(HexCell cell, CardManager creator)
    {
        base.OnCreated(cell, creator);
        UpdateBuffType(CellBuffType.BallLightning);
        InGameManager.instance.GameStateChangeEvent += OnGameStateChangeInBallLightningBuff;
    }

    private void OnGameStateChangeInBallLightningBuff()
    {
        if (InGameManager.isGameState(GameStateType.BeforeMove))
        {
            int needDamageAmount = DamageBuildingAmount;
            foreach (var nearbyCell in Cell.NearbyCells)
            {
                if(nearbyCell.OwnerManager != Creator)
                {
                    if(nearbyCell.PlacedBuilding != null)
                    {
                        Building_Base.DestroyBuilding(nearbyCell.PlacedBuilding);
                        needDamageAmount--;
                    }
                }

                if(needDamageAmount <= 0)
                {
                    return;
                }
            }
        } 
    }

    public override void OnBuffDestroy()
    {
        base.OnBuffDestroy();
        InGameManager.instance.GameStateChangeEvent -= OnGameStateChangeInBallLightningBuff;
    }
}
