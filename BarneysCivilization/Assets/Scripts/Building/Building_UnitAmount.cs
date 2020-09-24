using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_UnitAmount : Building_Base
{
    public int Amount;
    public override void OnCreated(HexCell cell, CardManager owner)
    {
        base.OnCreated(cell, owner);
        Cell.MinUnitAmount += Amount;
    }
    public override void OnBuildingDestroy()
    {
        base.OnBuildingDestroy();
        Cell.MinUnitAmount -= Amount;
    }
}
