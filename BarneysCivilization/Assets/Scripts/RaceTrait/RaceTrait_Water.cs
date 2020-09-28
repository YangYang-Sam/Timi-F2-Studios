using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceTrait_Water : RaceTrait_Base
{
    private void Start()
    {
        InGameManager.instance.LateDecisionEvent += OnLateDecision; ;
    }

    private void OnLateDecision()
    {
        
        foreach(Unit_Base unit in owner.Units)
        {

        }
    }
}
