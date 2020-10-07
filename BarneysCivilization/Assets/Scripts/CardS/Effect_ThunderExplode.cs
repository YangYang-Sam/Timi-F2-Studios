using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_ThunderExplode : CardEffect
{
    public int Turns;
    public GameObject BuffPrefab;
    public override bool CanUseCard(CardManager user, HexCell cell)
    {
        return base.CanUseCard(user, cell) && cell.GetUnitOnCell().Level>=3;
    }
    public override List<HexCell> GetCanUseCells(CardManager user)
    {
        return user.GetAllNearbyCells();
    }
    public override UseCardFailReason GetFailReason(CardManager user, HexCell cell)
    {
        if(cell.OwnerManager==user && cell.GetUnitOnCell().Level < 3)
        {
            return UseCardFailReason.InvalidUnitLevel;
        }
        return base.GetFailReason(user, cell);
    }
    public override void Effect(CardManager user, HexCell cell)
    {
        GameObject g = Instantiate(BuffPrefab, cell.transform.position, Quaternion.identity);
        CellBuff_Base buff = g.GetComponent<CellBuff_Base>();
        buff.Turns = Turns;
        buff.OnCreated(cell, user);
    }
}
