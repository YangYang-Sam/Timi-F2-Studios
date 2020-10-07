using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_RainStorm : CardEffect
{
    public int Turns;
    public int ResourceReduceAmount = 1;
    public GameObject BuffPrefab;
    public int NormalDamage = 2;
    public int HeavyDamage = 4;

    public override UseCardFailReason GetFailReason(CardManager user, HexCell cell)
    {
        if (user.ResourceAmount<0)
        {
            return UseCardFailReason.NoResource;
        }
        return base.GetFailReason(user, cell);
    }
    public override bool CanUseCard(CardManager user, HexCell cell)
    {
        return base.CanUseCard(user, cell) && (user.GetTotalResource() >= 0);
    }

    public override List<HexCell> GetCanUseCells(CardManager user)
    {
        List<HexCell> cells = new List<HexCell>(user.GetAllNearbyCells());

        if(user.GetTotalResource() < ResourceReduceAmount)
        {
            return cells;
        }
        return cells;
    }

    public override void Effect(CardManager user, HexCell cell)
    {
        user.TempResourceAmount -= ResourceReduceAmount;
        GameObject g = Instantiate(BuffPrefab, cell.transform.position, Quaternion.identity);
        CellBuff_RainStorm buff = g.GetComponent<CellBuff_RainStorm>();
        buff.Turns = Turns;
        if (cell.FindBuff(CellBuffType.Rain))
        {
            buff.Damage = HeavyDamage;
        }
        else
        {
            buff.Damage = NormalDamage;
        }
        buff.OnCreated(cell, user);
    }
}
