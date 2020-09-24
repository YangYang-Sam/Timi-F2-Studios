using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_AttackArea : Building_Base
{
    public int AllyAmount;
    public int EnemyAmount;

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
        List<HexCell> CheckList = new List<HexCell>();
        CheckList.Add(Cell);
        foreach (HexCell neighbor in Cell.NearbyCells)
        {
            CheckList.Add(neighbor);
        }

        foreach (HexCell checkCell in CheckList)
        {
            if (checkCell.OwnerManager == Owner)
            {
                checkCell.GetUnitOnCell().AddTempHealth(AllyAmount);
            }
            else if (checkCell.OwnerManager != null)
            {
                checkCell.GetUnitOnCell().AddTempHealth(-EnemyAmount);
            }
        }
    }


}
