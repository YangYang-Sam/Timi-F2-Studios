using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBuff_Superconduct : CellBuff_Base
{
    public int SpeedAddAmount = 5;
    public int HealthAddApmount = 2;
       
    public override void OnCreated(HexCell cell, CardManager creator)
    {
        base.OnCreated(cell, creator);
        UpdateBuffType(CellBuffType.Superconduct);
        InGameManager.instance.GameStateChangeEvent += OnGameStateChangeInSuperconductBuff;
    }

    private void OnGameStateChangeInSuperconductBuff()
    {
        if(InGameManager.isGameState(GameStateType.BeforeMove))
        {
            if(Cell.OwnerManager == Creator)
            {
                foreach(var unit in Cell.PlacedUnits)
                {
                    if(unit.Owner == Creator)
                    {
                        //unit.MoveSpeed += SpeedAddAmount; //无法针对单个unit更改移动速度，目前速度在CardManager上
                    }
                }
            }
        }
           
        if (InGameManager.isGameState(GameStateType.BeforeBattle))
        {
            if (Cell.OwnerManager == Creator)
            {
                foreach (var unit in Cell.PlacedUnits)
                {
                    if (unit.Owner == Creator)
                    {
                        unit.ChangeHealth(HealthAddApmount, Creator.GetCorePosition());
                    }
                }
            }
        }
    }

    public override void OnBuffDestroy()
    {
        base.OnBuffDestroy();
        InGameManager.instance.GameStateChangeEvent -= OnGameStateChangeInSuperconductBuff;
    }
}
