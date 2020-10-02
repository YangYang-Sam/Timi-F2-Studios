using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBuff_Nourish : CellBuff_Base
{
    public int HealthAddAmount = 1;
    
    public override void OnCreated(HexCell cell, CardManager creator)
    {
        base.OnCreated(cell, creator);
        UpdateBuffType(CellBuffType.Nourish);
        InGameManager.instance.GameStateChangeEvent += OnGameStateChangeInNourishBuff;
    }

    private void OnGameStateChangeInNourishBuff()
    {
        if (InGameManager.isGameState(GameStateType.AfterBattle) && Cell.OwnerManager == Creator && Cell.GetUnitOnCell())
        {
            foreach(var buff in Cell.CellBuffs)
            {
                if(buff.BuffType == CellBuffType.Rain)
                {
                    Cell.GetUnitOnCell().ChangeHealth(HealthAddAmount);
                    break;
                }
            }
        }
    }

    public override void OnBuffDestroy()
    {
        base.OnBuffDestroy();
        InGameManager.instance.BeforeTurnEndEvent -= OnGameStateChangeInNourishBuff;
    }
}
