using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_IceStorm : CardEffect
{
    public int Turns = 1;
    public int MinLevel = 2;
    public GameObject BuffPrefab;

    public override bool CanUseCard(CardManager user, HexCell cell)
    {
        return base.CanUseCard(user, cell) && cell.GetUnitOnCell() && (cell.GetUnitOnCell().Owner == user) && (cell.GetUnitOnCell().Level >= MinLevel);
    }
    public override UseCardFailReason GetFailReason(CardManager user, HexCell cell)
    {
        if (cell.GetUnitOnCell() && (cell.GetUnitOnCell().Level < MinLevel))
        {
            return UseCardFailReason.InvalidUnitLevel;
        }
     
        return base.GetFailReason(user, cell);
    }
    public override List<HexCell> GetCanUseCells(CardManager user)
    {
        List<HexCell> cells = new List<HexCell>();
        foreach(var unit in user.Units)
        {
            if(unit.Level >= MinLevel && !cells.Contains(unit.Cell))
            {
                cells.Add(unit.Cell);
            }
        }
        return cells;
    }

    public override void Effect(CardManager user, HexCell cell)
    {
        GameObject g = Instantiate(BuffPrefab, cell.transform.position, Quaternion.identity);
        CellBuff_Base buff = g.GetComponent<CellBuff_Base>();
        buff.Turns = Turns;
        buff.OnCreated(cell, user);
    }
}
