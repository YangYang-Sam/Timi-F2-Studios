using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBuff_Fire : CellBuff_Base
{
    public int Damage;
    public override void OnCreated(HexCell cell, CardManager creator)
    {
        base.OnCreated(cell, creator);
        UpdateBuffType(CellBuffType.Fire);
        InGameManager.instance.BeforeTurnEndEvent += OnBeforeTurnEnd;
    }

    private void OnBeforeTurnEnd()
    {
        if (Cell.GetUnitOnCell() != null)
        {
            Cell.GetUnitOnCell().TakeDamage(Damage, null);
        }
    }

    public override void OnBuffDestroy()
    {
        base.OnBuffDestroy();
        InGameManager.instance.BeforeTurnEndEvent -= OnBeforeTurnEnd;
    }
}
