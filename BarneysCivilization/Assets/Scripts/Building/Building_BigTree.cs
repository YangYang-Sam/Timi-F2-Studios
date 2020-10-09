using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_BigTree : Building_Base 
{
    public override void OnCreated(HexCell cell, CardManager owner)
    {
        base.OnCreated(cell, owner);
        InGameManager.instance.LateDecisionEvent += OnLateDecision;
    }

    private void OnLateDecision()
    {
        if(Cell.GetUnitOnCell())
        {
            Cell.GetUnitOnCell().ChangeHealth(1, transform.position);
            if (Cell.GetUnitOnCell().Level == 3)
            {
                OnBuildingDestroy();
            }
            else
            {
                Cell.GetUnitOnCell().CanMove = false;
            }
        }
    }

    public override void OnBuildingDestroy()
    {
        InGameManager.instance.LateDecisionEvent -= OnLateDecision;
        base.OnBuildingDestroy();  
    }
}
