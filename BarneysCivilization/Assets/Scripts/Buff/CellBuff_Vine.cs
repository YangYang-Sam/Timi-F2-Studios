using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBuff_Vine : CellBuff_Base
{
    public override void OnCreated(HexCell cell, CardManager creator)
    {
        base.OnCreated(cell, creator);
        UpdateBuffType(CellBuffType.Vine);

        foreach (var nearbyCell in Cell.NearbyCells)
        {
            if (nearbyCell && (nearbyCell.OwnerManager != Creator) && nearbyCell.GetUnitOnCell())
            {
                nearbyCell.GetUnitOnCell().CanMove = false;
            }
        }

        if (Cell && (Cell.OwnerManager != Creator) && Cell.GetUnitOnCell())
        {
            Cell.GetUnitOnCell().CanMove = false;
        }
    }

    public override void OnBuffDestroy()
    {
        base.OnBuffDestroy();
    }
}
