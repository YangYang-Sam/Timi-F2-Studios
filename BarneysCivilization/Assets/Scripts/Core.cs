using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : Building_Base
{
    public override void OnCreated(HexCell cell, CardManager owner)
    {
        base.OnCreated(cell, owner);
    }
    public override void OnBuildingDestroy()
    {
        base.OnBuildingDestroy();
        Owner.CampLost(Cell.OwnerManager);
    }
}
