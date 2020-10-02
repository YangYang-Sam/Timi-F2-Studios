using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_PlayerBuff : CardEffect
{
    public int Turns;
    public GameObject BuffPrefab;
    public override bool CanUseCard(CardManager user, HexCell cell)
    {
        return base.CanUseCard(user, cell);
    }
    public override List<HexCell> GetCanUseCells(CardManager user)
    {
        return null;
    }
    public override void Effect(CardManager user, HexCell cell)
    {
        GameObject g = Instantiate(BuffPrefab);
        PlayerBuff_Base buff = g.GetComponent<PlayerBuff_Base>();
        buff.Turns = Turns;
        buff.OnCreated(user, user);
    }
}
