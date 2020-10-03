using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceTrait_Fire : RaceTrait_Base
{
    private void Start()
    {
        owner.OccupyNewCellEvent += OnOccupyNewCell;
    }

    private void OnOccupyNewCell(HexCell cell)
    {
        if (cell.CellType == HexCellType.Forest || cell.CellType== HexCellType.Grass)
        {
            cell.GetUnitOnCell().ChangeHealth(1);
        }
    }
}
