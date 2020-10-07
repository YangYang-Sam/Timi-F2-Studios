using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_StonePrison : CardEffect
{
    public int LockedTurns = 1;
    public GameObject BuffPrefab;

    bool IsCellHasCore(HexCell cell)
    {
        foreach (var manager in InGameManager.instance.CardManagers)
        {
            if (manager.PlayerCore.Cell == cell)
            {
                return true;
            }
        }

        return false;
    }
    public override UseCardFailReason GetFailReason(CardManager user, HexCell cell)
    {
        if (IsCellHasCore(cell))
        {
            return UseCardFailReason.CantUseOnCore;
        }
        return base.GetFailReason(user, cell);
    }
    public override bool CanUseCard(CardManager user, HexCell cell)
    {
        return base.CanUseCard(user, cell) && !IsCellHasCore(cell);
    }

    public override List<HexCell> GetCanUseCells(CardManager user)
    {
        List<HexCell> cells = new List<HexCell>();

        foreach(var cell in user.GetAllNearbyCells())
        {
            if(!IsCellHasCore(cell))
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
        buff.Turns = LockedTurns + 1;
        buff.OnCreated(cell, user);
    }
}
