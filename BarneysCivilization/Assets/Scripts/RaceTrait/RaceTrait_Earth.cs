using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceTrait_Earth :RaceTrait_Base
{
    public Color c;
    private void Start()
    {
        InGameManager.instance.BeforeTurnEndEvent += OnTurnEnd;
    }

    private void OnTurnEnd()
    {
        foreach (Unit_Base unit in owner.Units)
        {           
            if (!unit.CanMove)
            {
                unit.ChangeHealth(1, owner.GetCorePosition());
                ArtResourceManager.instance.CreateTextEffect("不动如山", unit.transform.position, c, 1);
            }
        }
    }
}
