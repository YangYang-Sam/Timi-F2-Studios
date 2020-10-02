﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Burn : CardEffect
{
    public int FireTurns = 2;
    public int HealthAddAmount = 1;
    public GameObject BuffPrefab;

    public bool IsSupportCellType(HexCell cell)
    {
        return (cell.CellType == HexCellType.Forest) || (cell.CellType == HexCellType.Grass);
    }

    public override bool CanUseCard(CardManager user, HexCell cell)
    {
        return base.CanUseCard(user, cell) && (cell.OwnerManager == user) && IsSupportCellType(cell);
    }

    public override List<HexCell> GetCanUseCells(CardManager user)
    {
        List<HexCell> cells = new List<HexCell>();
        foreach (var cell in user.OccupiedCells)
        {
            if(IsSupportCellType(cell))
            {
                cells.Add(cell);
            }
        }      
        return cells;
    }

    public override void Effect(CardManager user, HexCell cell)
    {
        cell.GetUnitOnCell().ChangeHealth(HealthAddAmount);
        GameObject g = Instantiate(BuffPrefab, cell.transform.position, Quaternion.identity);
        CellBuff_Base buff = g.GetComponent<CellBuff_Base>();
        buff.Turns = FireTurns;
        buff.OnCreated(cell, user);
    }
}
