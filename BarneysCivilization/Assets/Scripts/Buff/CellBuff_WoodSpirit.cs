using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBuff_WoodSpirit : CellBuff_Base
{
    public List<CardManager> Creators = new List<CardManager>();
    public override void OnCreated(HexCell cell, CardManager creator)
    {
        base.OnCreated(cell, creator);
        Creators.Add(creator);
    }
    public override void OnStack(CellBuff_Base newBuff)
    {
        base.OnStack(newBuff);
        if (!Creators.Contains(newBuff.Creator))
        {
            Creators.Add(newBuff.Creator);
        }
    }
}
