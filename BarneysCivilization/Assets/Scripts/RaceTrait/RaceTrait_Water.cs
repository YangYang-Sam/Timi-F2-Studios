using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceTrait_Water : RaceTrait_Base
{
    private void Start()
    {
        InGameManager.instance.BeforeTurnEndEvent += OnBeforeTurnEnd;
    }

    private void OnBeforeTurnEnd()
    {        
        foreach(Unit_Base unit in owner.Units)
        {
            CellBuff_Base oldBuff = unit.Cell.FindBuff(CellBuffType.Rain);
            if (oldBuff != null)
            {
                unit.ChangeHealth(1, owner.GetCorePosition());
                ArtResourceManager.instance.CreateTextEffect("高涨之水", unit.transform.position, new Color(0.7f,1, 0.9424f,1), 1);
            }            
        }
    }
}
