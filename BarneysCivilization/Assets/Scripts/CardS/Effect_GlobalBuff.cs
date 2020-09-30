using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_GlobalBuff : CardEffect
{
    public int Turns;
    public GameObject BuffPrefab;
    public override bool CanUseCard(CardManager user, HexCell cell)
    {
        return base.CanUseCard(user, cell);
    }
    public override List<HexCell> GetCanUseCells(CardManager user)
    {
        return base.GetCanUseCells(user);
    }
    public override void Effect(CardManager user, HexCell cell)
    {
       foreach(var occupiedCell in user.OccupiedCells)
       {
            GameObject g = Instantiate(BuffPrefab, occupiedCell.transform.position, Quaternion.identity);
            CellBuff_Base buff = g.GetComponent<CellBuff_Base>();
            buff.Turns = Turns;
            buff.OnCreated(cell, user);
        }
    }
}
