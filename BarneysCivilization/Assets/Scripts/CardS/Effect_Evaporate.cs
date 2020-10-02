using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Evaporate : CardEffect
{
    public int RainTurns = 4;
    public int HealthReduceAmount = 2;
    public int ResourceAddAmount = 2;
    public GameObject BuffPrefab;
    public override bool CanUseCard(CardManager user, HexCell cell)
    {
        return base.CanUseCard(user, cell) && (cell.GetUnitOnCell().Health > HealthReduceAmount);
    }
    public override List<HexCell> GetCanUseCells(CardManager user)
    {
        List<HexCell> cells = new List<HexCell>();

        foreach (var cell in user.OccupiedCells)
        {
            if(cell.GetUnitOnCell().Health > HealthReduceAmount)
            {
                cells.Add(cell);
            }            
        }

        return cells;
    }
    public override void Effect(CardManager user, HexCell cell)
    {
        cell.GetUnitOnCell().TakeDamage(HealthReduceAmount, null);
        user.TempResourceAmount += ResourceAddAmount;
        GameObject g = Instantiate(BuffPrefab, cell.transform.position, Quaternion.identity);
        CellBuff_Base buff = g.GetComponent<CellBuff_Base>();
        buff.Turns = RainTurns;
        buff.OnCreated(cell, user);
    }
}
