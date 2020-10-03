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
                        unit.ChangeHealth(Mathf.Min(unit.MoveDistance, 3));
                    }
                }
                break;
        }
    }
}
