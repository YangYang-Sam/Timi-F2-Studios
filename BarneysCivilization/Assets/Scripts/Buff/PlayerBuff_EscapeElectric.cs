using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuff_EscapeElectric : PlayerBuff_Base
{
    public int EffectIndex = 9;
    private void OnDeathEvent(int damage, Unit_Base unit)
    {
        Creator.TempResourceAmount += damage / 2;
        ArtResourceManager.instance.CreateTextEffect("散逸电荷", unit.transform.position);
        ArtResourceManager.instance.CreateEffectByIndex(unit.transform.position, EffectIndex);
    }

    protected override void OnUnitBeforeBattle(Unit_Base unit, HexCell cell)
    {
        base.OnUnitBeforeBattle(unit, cell);
        unit.DeathEvent += OnDeathEvent;
    }
}
