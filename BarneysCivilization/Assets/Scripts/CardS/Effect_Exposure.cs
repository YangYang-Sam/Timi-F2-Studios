using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Exposure : CardEffect
{
    public int FireTurns = 2;
    public int ActionPointAddAmount = 1;
    public GameObject BuffPrefab;

    public override bool CanUseCard(CardManager user, HexCell cell)
    {
        return base.CanUseCard(user, cell);
    }

    public override List<HexCell> GetCanUseCells(CardManager user)
    {
        return user.GetAllNearbyCells();
    }

    public override void Effect(CardManager user, HexCell cell)
    {
       if((cell.CellType == HexCellType.Forest) || (cell.CellType == HexCellType.Grass))
        {
            user.ActionPoint += ActionPointAddAmount;
        }

        GameObject g = Instantiate(BuffPrefab, cell.transform.position, Quaternion.identity);
        CellBuff_Base buff = g.GetComponent<CellBuff_Base>();
        buff.Turns = FireTurns;
        buff.OnCreated(cell, user);
    }
}
