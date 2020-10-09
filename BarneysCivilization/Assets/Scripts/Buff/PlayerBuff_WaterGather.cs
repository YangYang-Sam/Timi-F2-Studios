using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuff_WaterGather : PlayerBuff_Base
{
    public List<HexCellType> Types;
    public int AmountPerCell = 1;
    public int AmountPerRain = 1;
    public int EffectIndex=1;
    protected override void OnUnitBeforeBattle(Unit_Base unit, HexCell cell)
    {
        base.OnUnitBeforeBattle(unit, cell);
        if (unit.Level > 1)
        {
            int amount = 0;
            ArtResourceManager.instance.CreateEffectByIndex(unit.transform.position, EffectIndex);
            ArtResourceManager.instance.CreateTextEffect("汇聚", unit.transform.position);

            if (Types.Contains(cell.CellType))
            {
                amount += AmountPerCell;
            }
            if (cell.FindBuff(CellBuffType.Rain) != null)
            {
                amount += AmountPerRain;
            }
            unit.ChangeHealth(amount,cell.transform.position);
            foreach (HexCell neighbor in cell.NearbyCells)
            {
                amount = 0;
                if (Types.Contains(neighbor.CellType))
                {
                    amount+= AmountPerCell;
                }
                if (neighbor.FindBuff(CellBuffType.Rain) != null)
                {
                    amount += AmountPerRain;
                }
                unit.ChangeHealth(amount,neighbor.transform.position);
            }
            
        }
    }
}
