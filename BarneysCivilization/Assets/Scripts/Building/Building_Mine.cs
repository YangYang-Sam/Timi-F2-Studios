using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Mine : Building_Base
{
    public int ResourceAmount = 1;

    public override void OnCreated(HexCell cell, CardManager owner)
    {
        base.OnCreated(cell, owner);
        InGameManager.instance.LateDecisionEvent += OnLateDecision;
    }
    public override void OnBuildingDestroy()
    {
        base.OnBuildingDestroy();
        InGameManager.instance.LateDecisionEvent -= OnLateDecision;
    }
    private void OnLateDecision()
    {
        Owner.TempResourceAmount += ResourceAmount;
    }


}
