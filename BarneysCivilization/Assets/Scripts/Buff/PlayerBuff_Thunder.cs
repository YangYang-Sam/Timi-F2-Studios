using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuff_Thunder : PlayerBuff_Base
{
    public int DamageHealthAmount = 2;
    private List<HexCell> BattleCells = new List<HexCell>();

    protected override void OnUnitBeforeBattle(Unit_Base unit, HexCell cell)
    {
        base.OnUnitBeforeBattle(unit, cell);

        if(!BattleCells.Contains(cell))
        {
            BattleCells.Add(cell);
        }
    }

    protected override void OnGameStateChange()
    {
        base.OnGameStateChange();

        if (InGameManager.isGameState(GameStateType.AfterBattle))
        {
            foreach(var cell in BattleCells)
            {
                if(cell && cell.GetUnitOnCell() && cell.GetUnitOnCell().Owner != Creator)
                {
                    cell.GetUnitOnCell().TakeDamage(DamageHealthAmount, null);
                }
            }

            BattleCells.Clear();
        }
    }
}
