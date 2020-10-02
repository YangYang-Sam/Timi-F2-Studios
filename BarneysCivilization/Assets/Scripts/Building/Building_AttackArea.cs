using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_AttackArea : Building_Base
{
    public int Amount;
    public GameObject WoodSpiritBuff;

    public override void OnCreated(HexCell cell, CardManager owner)
    {
        base.OnCreated(cell, owner);

        List<HexCell> CheckList = new List<HexCell>();
        CheckList.Add(Cell);
        foreach (HexCell neighbor in Cell.NearbyCells)
        {
            CheckList.Add(neighbor);
        }

        foreach (HexCell checkCell in CheckList)
        {
            checkCell.CellBeforeBattleEvent += BeforeBattle;            
        }

    }

    private void BeforeBattle(HexCell cell)
    {
        CellBuff_Base buff = cell.FindBuff(CellBuffType.WoodSpirit);
        if (buff == null)
        {
            foreach(Unit_Base unit in cell.PlacedUnits)
            {
                if (unit.Owner == Owner)
                {
                    unit.ChangeHealth(Amount);
                    AddBuff(cell);
                    break;
                }
            }
        }
    }

    public override void OnBuildingDestroy()
    {
        base.OnBuildingDestroy(); 
    }
 
    public void AddBuff(HexCell cell)
    {
        GameObject g = Instantiate(WoodSpiritBuff, cell.transform.position, Quaternion.identity);
        CellBuff_Base buff = g.GetComponent<CellBuff_Base>();
        buff.Turns = Turns;
        buff.OnCreated(cell, Owner);
    }

}
