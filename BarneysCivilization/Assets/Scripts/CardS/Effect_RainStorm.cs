using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_RainStorm : CardEffect
{
    public int Turns;
    public int ResourceReduceAmount = 1;
    public GameObject BuffPrefab;

    private bool CanUseRainStormCell(CardManager user, HexCell targetCell)
    {
        bool isUserOccupiedCell = false;
        if (targetCell != null)
        {
            isUserOccupiedCell = targetCell.OwnerManager == user;
        }
        bool isSupportCellType = (targetCell.CellType == HexCellType.Forest) || (targetCell.CellType == HexCellType.Water);
        foreach(var buff in targetCell.CellBuffs)
        {
            if(buff.BuffType == CellBuffType.Rain)
            {
                isSupportCellType |= true;
                break;
            }
        }
        return !isUserOccupiedCell && isSupportCellType;
    }

    public override bool CanUseCard(CardManager user, HexCell cell)
    {
        return base.CanUseCard(user, cell) && (user.GetTotalResource() >= ResourceReduceAmount) && CanUseRainStormCell(user, cell);
    }

    public override List<HexCell> GetCanUseCells(CardManager user)
    {
        List<HexCell> cells = new List<HexCell>();

        if(user.GetTotalResource() < ResourceReduceAmount)
        {
            return cells;
        }

        foreach(var cell in user.GetAllNearbyCells())
        {
            if(CanUseRainStormCell(user, cell))
            {
                cells.Add(cell);
            }
        }

        return cells;
    }

    public override void Effect(CardManager user, HexCell cell)
    {
        user.TempResourceAmount -= ResourceReduceAmount;
        GameObject g = Instantiate(BuffPrefab, cell.transform.position, Quaternion.identity);
        CellBuff_Base buff = g.GetComponent<CellBuff_Base>();
        buff.Turns = Turns;
        buff.OnCreated(cell, user);
    }
}
