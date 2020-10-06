using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuff_Superconduct : PlayerBuff_Base
{
    public int SpeedAddAmount = 2;
    public int HealthAddApmount = 2;

    public override void OnCreated(CardManager owner, CardManager creator)
    {
        base.OnCreated(owner, creator);
        Creator.UnitMoveSpeed += SpeedAddAmount;
    }

    protected override void OnUnitBeforeBattle(Unit_Base unit, HexCell cell)
    {
        base.OnUnitBeforeBattle(unit, cell);

        unit.ChangeHealth(HealthAddApmount, Creator.GetCorePosition());
    }

}
