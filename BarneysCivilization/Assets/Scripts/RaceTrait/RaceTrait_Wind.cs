using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceTrait_Wind : RaceTrait_Base
{
    public Color c;
    private void Start()
    {
        InGameManager.instance.LateDecisionEvent += OnLateDecision;
    }

    private void OnLateDecision()
    {
        foreach (Unit_Base unit in owner.Units)
        {
            bool isolated=true;
            foreach(HexCell cell in unit.Cell.NearbyCells)
            {
                if (cell.OwnerManager == owner)
                {
                    isolated = false;
                    break;
                }
            }
            if (isolated)
            {
                unit.ChangeHealth(1, owner.GetCorePosition());
                ArtResourceManager.instance.CreateTextEffect("独行之风", unit.transform.position, c, 1);
            }
        }
    }
}
