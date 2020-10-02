﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuff_WaterGather : PlayerBuff_Base
{
    public List<HexCellType> Types;
    public int AmountPerCell = 1;
    public int AmountPerRain = 1;
    protected override void OnUnitBeforeBattle(Unit_Base unit, HexCell cell)
    {
        base.OnUnitBeforeBattle(unit, cell);
        if (unit.Level > 1)
        {
            int amount = 0;

            if (Types.Contains(cell.CellType))
            {
                amount += AmountPerCell;
            }
            if (cell.FindBuff(CellBuffType.Rain) != null)
            {
                amount += AmountPerRain;
            }

            foreach (HexCell neighbor in cell.NearbyCells)
            {
                if (Types.Contains(neighbor.CellType))
                {
                    amount+= AmountPerCell;
                }
                if (neighbor.FindBuff(CellBuffType.Rain) != null)
                {
                    amount += AmountPerRain;
                }
            }
            unit.ChangeHealth(amount);
        }
    }
}