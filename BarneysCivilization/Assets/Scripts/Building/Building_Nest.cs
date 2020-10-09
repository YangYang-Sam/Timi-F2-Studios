using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Nest : Building_Base
{
    public int ResourceAmount = 1;

    public override void OnCreated(HexCell cell, CardManager owner)
    {
        base.OnCreated(cell, owner);
        InGameManager.instance.LateDecisionEvent += OnLateDecision;
    }

    private void OnLateDecision()
    {
        bool isolate = true;
        foreach(HexCell cell in Cell.NearbyCells)
        {
            if (cell.OwnerManager == Owner)
            {
                isolate=false;
                break;
            }
        }
        if (isolate && Cell.GetUnitOnCell())
        {
            Cell.GetUnitOnCell().ChangeHealth(1, EffectTransform.position);
        }
    }

    public override void OnBuildingDestroy()
    {
        base.OnBuildingDestroy();
        InGameManager.instance.LateDecisionEvent -= OnLateDecision;
    }
}
