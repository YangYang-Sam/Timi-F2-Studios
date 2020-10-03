using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuff_BurnToGround : PlayerBuff_Base
{
    public int AmountPerAlly = 1;
    public int AmountPerFire = 1;
    protected override void OnUnitBeforeBattle(Unit_Base unit, HexCell cell)
    {
        base.OnUnitBeforeBattle(unit, cell);
        if (unit.Level > 1)
        {
            int amount = 0;

 
            if (cell.FindBuff(CellBuffType.Rain) != null)
            {
                amount += AmountPerFire;
                unit.ChangeHealth(AmountPerFire, Cell.transform.position);                
            }

            foreach (HexCell neighbor in cell.NearbyCells)
            {
                amount = 0;
                if (neighbor.OwnerManager==Owner)
                {
                    amount += AmountPerAlly;
                
                }
                if (neighbor.FindBuff(CellBuffType.Fire) != null)
                {
                    amount += AmountPerFire;
                }
                unit.ChangeHealth(amount, neighbor.transform.position);
            }
            
        }
    }
}
