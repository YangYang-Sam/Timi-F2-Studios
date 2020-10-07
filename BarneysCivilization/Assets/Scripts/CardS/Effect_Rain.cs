using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Rain : CardEffect
{
    public int Turns = 3;
    public int ActionPointAddAmount = 1;
    public int ResourceReduceAmount = 1;
    public GameObject BuffPrefab;
    public override bool CanUseCard(CardManager user, HexCell cell)
    {
        return base.CanUseCard(user, cell) && (user.GetTotalResource() >= 0);
    }
    public override UseCardFailReason GetFailReason(CardManager user, HexCell cell)
    {
        if (user.GetTotalResource() < 0)
        {
            return UseCardFailReason.NoResource;
        }
        return base.GetFailReason(user, cell);
    }
    public override List<HexCell> GetCanUseCells(CardManager user)
    {
        List<HexCell> cells = new List<HexCell>();

        if (user.GetTotalResource() < ResourceReduceAmount)
        {
            return cells;
        }

        foreach (var cell in user.GetAllNearbyCells())
        {
            cells.Add(cell);
        }

        return cells;
    }
    public override void Effect(CardManager user, HexCell cell)
    {
        user.ActionPoint += ActionPointAddAmount;
        user.TempResourceAmount -= ResourceReduceAmount;
        GameObject g = Instantiate(BuffPrefab, cell.transform.position, Quaternion.identity);
        CellBuff_Base buff = g.GetComponent<CellBuff_Base>();
        buff.Turns = Turns;
        buff.OnCreated(cell, user);
    }
}
