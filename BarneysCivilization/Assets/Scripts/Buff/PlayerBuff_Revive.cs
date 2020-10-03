using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuff_Revive : PlayerBuff_Base
{
    public int amount = 3;
    protected override void OnUnitWinBattle(Unit_Base unit, HexCell cell)
    {
        base.OnUnitWinBattle(unit, cell);
        unit.ChangeHealth(Mathf.Min(amount, unit.LastDamage),cell.transform.position);
    }
}
