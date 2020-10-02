using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_ThunderSlash : CardEffect
{
    public int Turns = 1;
    public GameObject BuffPrefab;

    public bool IsSupportCell(CardManager user, HexCell cell)
    {
        if (cell && cell.OwnerManager && (cell.OwnerManager != user))
        {
            return true;
        }

        return false;
    }

    public override bool CanUseCard(CardManager user, HexCell cell)
    {
        return base.CanUseCard(user, cell) && IsSupportCell(user, cell);
    }

    public override List<HexCell> GetCanUseCells(CardManager user)
    {
        List<HexCell> cells = new List<HexCell>();

        foreach (var cell in user.GetAllNearbyCells())
        {
            if (IsSupportCell(user, cell))
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
    }
}
