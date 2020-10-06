using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceTrait_Lightning : RaceTrait_Base
{
    int currentTimes;
    int MaxTimes = 3;
    private void Start()
    {
        InGameManager.instance.GameStateChangeEvent += OnGameStateChange;
        owner.UnitVisitCellEvent += OnUnitVisitCell;
    }

    private void OnUnitVisitCell(Unit_Base unit, HexCell cell)
    {
        if (unit.Level >= 2 && currentTimes < MaxTimes)
        {
            ArtResourceManager.instance.CreateTextEffect("静电吸附", cell.transform.position);
            unit.ChangeHealth(1, cell.transform.position);
            currentTimes++;
        }
      
    }

    private void OnGameStateChange()
    {
        switch (InGameManager.CurrentGameState)
        {
            case GameStateType.Decision:
                currentTimes = 0;
                break;
        }
    }
}
