using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceTrait_Lightning : RaceTrait_Base
{
    private void Start()
    {
        InGameManager.instance.GameStateChangeEvent += OnGameStateChange;
    }

    private void OnGameStateChange()
    {
        switch (InGameManager.CurrentGameState)
        {
            case GameStateType.BeforeBattle:
                foreach(Unit_Base unit in owner.Units)
                {
                    if (unit.Level > 1)
                    {
                        for (int i = 0; i < unit.VisitCells.Count; i++)
                        {
                            unit.ChangeHealth(Mathf.Min(unit.MoveDistance, 1), unit.VisitCells[i].transform.position);
                            if (i >= 2)
                            {
                                break;
                            }
                        }                       
                    }
                }
                break;
        }
    }
}
