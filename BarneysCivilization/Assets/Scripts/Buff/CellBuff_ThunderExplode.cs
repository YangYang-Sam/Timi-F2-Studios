using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBuff_ThunderExplode : CellBuff_Base
{
    public int damage = 2;
    public int EffectIndex = 11;
    public override void OnCreated(HexCell cell, CardManager creator)
    {
        base.OnCreated(cell, creator);
        UpdateBuffType(CellBuffType.ThunderExplode);
    }
    protected override void OnGameStateChange()
    {
        base.OnGameStateChange();
        if (InGameManager.isGameState(GameStateType.BeforeMove))
        {
            int amount = Cell.GetUnitOnCell().Health - 1;
            Cell.GetUnitOnCell().TakeDamage(amount, null);

            foreach (var nearbyCell in Cell.NearbyCells)
            {
                if (nearbyCell && (nearbyCell.OwnerManager != Creator) && nearbyCell.GetUnitOnCell())
                {
                    nearbyCell.GetUnitOnCell().TakeDamage(damage, Cell.GetUnitOnCell());
                }
            }

            Creator.TempResourceAmount += amount;
            ArtResourceManager.instance.CreateTextEffect("雷暴", Cell.transform.position);
            ArtResourceManager.instance.CreateEffectByIndex(transform.position, EffectIndex);
        }
    }

    public override void OnBuffDestroy()
    {
        base.OnBuffDestroy();
    }
}
