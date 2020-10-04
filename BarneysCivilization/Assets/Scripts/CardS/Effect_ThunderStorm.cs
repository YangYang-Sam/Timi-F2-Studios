using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_ThunderStorm : CardEffect
{
    public int Turns = 2;
    public int CenterHealthReduceAmount = 2;
    public GameObject BuffPrefab;

    public override bool CanUseCard(CardManager user, HexCell cell)
    {
        return base.CanUseCard(user, cell) && (cell.OwnerManager == user) && cell.GetUnitOnCell() && (cell.GetUnitOnCell().GetTotalHealth() > CenterHealthReduceAmount);
    }

    public override List<HexCell> GetCanUseCells(CardManager user)
    {
        List<HexCell> cells = new List<HexCell>();
        foreach (var cell in user.OccupiedCells)
        {
            if (cell.GetUnitOnCell() && (cell.GetUnitOnCell().GetTotalHealth() > CenterHealthReduceAmount))
            {
                cells.Add(cell);
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

        if (cell.GetUnitOnCell())
        {
            cell.GetUnitOnCell().TakeDamage(CenterHealthReduceAmount, null);
        }
    }
}
