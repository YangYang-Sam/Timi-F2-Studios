using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_SnowStorm : CardEffect
{
    public int Turns = 1;
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
        GameObject g = Instantiate(BuffPrefab, cell.transform.position, Quaternion.identity);
        CellBuff_Base buff = g.GetComponent<CellBuff_Base>();
        buff.Turns = Turns;
        buff.OnCreated(cell, user);

        if((cell.OwnerManager != user) && cell.GetUnitOnCell())
        {
            cell.GetUnitOnCell().canMove = false;
        }

        // CreateCannotMoveEffect没配注掉
        //ArtResourceManager.instance.CreateCannotMoveEffect(cell.transform.position);
        ArtResourceManager.instance.CreateTextEffect("暴风雪", cell.transform.position);
    }
}
