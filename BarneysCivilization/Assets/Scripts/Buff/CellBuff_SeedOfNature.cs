using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBuff_SeedOfNature : CellBuff_Base
{
    public override void OnCreated(HexCell cell, CardManager creator)
    {
        base.OnCreated(cell, creator);
        cell.CellBuildingCreateEvent += OnCellBuilding;
        cell.CellBuildingUpgradeEvent += OnCellBuilding;
    }

    protected override void OnGameStateChange()
    {
        base.OnGameStateChange();
        if (InGameManager.isGameState(GameStateType.Decision))
        {
            if (Cell.GetUnitOnCell() != null)
            {
                Cell.GetUnitOnCell().ChangeHealth(1);
            }
        }
    }
    private void OnCellBuilding()
    {
        Cell.PlacedBuilding.Owner.ActionPoint++;
        OnBuffDestroy();
    }

    public override void OnBuffDestroy()
    {
        base.OnBuffDestroy();
        Cell.CellBuildingCreateEvent -= OnCellBuilding;
        Cell.CellBuildingUpgradeEvent -= OnCellBuilding;
    }
}
