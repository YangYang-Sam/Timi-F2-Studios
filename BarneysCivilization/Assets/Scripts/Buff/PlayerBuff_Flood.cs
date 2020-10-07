using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuff_Flood : PlayerBuff_Base
{
    public int EffectIndex=2;
    protected override void OnGameStateChange()
    {
        base.OnGameStateChange();
        if (InGameManager.isGameState(GameStateType.BeforeBattle))
        {
            foreach(Unit_Base unit in Owner.Units)
            {
                if (unit.Level > 1)
                {
                    for (int i = unit.Cell.CellBuffs.Count - 1; i >= 0; i--)
                    {
                        if (unit.Cell.CellBuffs[i].Creator != Owner)
                        {
                            unit.Cell.CellBuffs[i].OnBuffDestroy();
                        }
                    }
                    if(unit.Cell.OwnerManager!=Owner && unit.Cell.PlacedBuilding != null)
                    {
                        unit.Cell.PlacedBuilding.OnBuildingDestroy();
                    }

                    ArtResourceManager.instance.CreateEffectByIndex(unit.transform.position, EffectIndex);
                    ArtResourceManager.instance.CreateTextEffect("洪水", unit.transform.position);
                }
            }
        }
    }
}
