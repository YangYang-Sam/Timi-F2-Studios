using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBuff_Overload : CellBuff_Base
{
    public int HealthAddApmount = 1;

    public override void OnCreated(HexCell cell, CardManager creator)
    {
        base.OnCreated(cell, creator);
        UpdateBuffType(CellBuffType.Overload);
        InGameManager.instance.GameStateChangeEvent += OnGameStateChangeInOverloadBuff;
    }

    private void OnGameStateChangeInOverloadBuff()
    {
        if (InGameManager.isGameState(GameStateType.BeforeMove) && Cell.GetUnitOnCell())
        {
            List<Unit_Base> units = new List<Unit_Base>();
            int num = Cell.GetUnitOnCell().Level;

            if(num > 0)
            {
                foreach(var nearbyCell in Cell.NearbyCells)
                {
                    if(nearbyCell.OwnerManager == Creator)
                    {
                        if(nearbyCell.GetUnitOnCell())
                        {
                            units.Add(Cell.GetUnitOnCell());
                            num--;
                        }
                    }

                    if(num <= 0)
                    {
                        break;
                    }
                }
            }

            if (num > 0)
            {
                foreach (var nearbyCell in Cell.NearbyCells)
                {
                    if (nearbyCell.OwnerManager != Creator)
                    {
                        if (nearbyCell.GetUnitOnCell())
                        {
                            units.Add(Cell.GetUnitOnCell());
                            num--;
                        }
                    }

                    if (num <= 0)
                    {
                        break;
                    }
                }
            }

            foreach(var unit in units)
            {
                unit.ChangeHealth(HealthAddApmount, Creator.GetCorePosition());
            }
        }
    }

    public override void OnBuffDestroy()
    {
        base.OnBuffDestroy();
        InGameManager.instance.GameStateChangeEvent -= OnGameStateChangeInOverloadBuff;
    }
}
