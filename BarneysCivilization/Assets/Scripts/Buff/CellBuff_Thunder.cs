using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBuff_Thunder : CellBuff_Base
{
    public int DamageHealthAmount = 2;
    private int NeedDamageHealthAmount = 0;

    public override void OnCreated(HexCell cell, CardManager creator)
    {
        base.OnCreated(cell, creator);
        UpdateBuffType(CellBuffType.Thunder);
        InGameManager.instance.GameStateChangeEvent += OnGameStateChangeInThunderBuff;
    }

    private void OnGameStateChangeInThunderBuff()
    {
        if (InGameManager.isGameState(GameStateType.BeforeBattle))
        {
            bool hasOwnerUnit = false;
            bool hasEnemyUnit = false;

            foreach (var unit in Cell.PlacedUnits)
            {
                if(unit.Owner == Creator)
                {
                    hasOwnerUnit = true;
                }
                else
                {
                    hasEnemyUnit = true;
                }
            }

            NeedDamageHealthAmount = 0;

            if (hasOwnerUnit && hasEnemyUnit)
            {
                NeedDamageHealthAmount = DamageHealthAmount;
            }
        }

        if (InGameManager.isGameState(GameStateType.AfterBattle))
        {
            if(Cell.GetUnitOnCell() && (Cell.GetUnitOnCell().Owner != Creator) && NeedDamageHealthAmount > 0)
            {
                Cell.GetUnitOnCell().TakeDamage(NeedDamageHealthAmount, null);
            }
        }
    }

    public override void OnBuffDestroy()
    {
        base.OnBuffDestroy();
        InGameManager.instance.GameStateChangeEvent -= OnGameStateChangeInThunderBuff;
    }
}
