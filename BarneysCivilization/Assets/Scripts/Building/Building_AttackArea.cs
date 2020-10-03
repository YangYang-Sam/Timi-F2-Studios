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
        CellBuff_WoodSpirit buff = cell.FindBuff(CellBuffType.WoodSpirit) as CellBuff_WoodSpirit;
        if (buff == null)
        {
            print("No Buff Exist");
        }
        else
        {
            print("Buff exist, Creators:"+buff.Creators.Count);
        }
        if (buff == null || !buff.Creators.Contains(Owner))
        {
            foreach(Unit_Base unit in cell.PlacedUnits)
            {
                if (unit.Owner == Owner)
                {
                    unit.ChangeHealth(Amount * Level,transform.position);
                    AddBuff(cell);
                    break;
                }
            }
        }
    }

    public override void OnBuildingDestroy()
    {
        base.OnBuildingDestroy();
        List<HexCell> CheckList = new List<HexCell>();
        CheckList.Add(Cell);
        foreach (HexCell neighbor in Cell.NearbyCells)
        {
            CheckList.Add(neighbor);
        }

        foreach (HexCell checkCell in CheckList)
        {
            checkCell.CellBeforeBattleEvent -= BeforeBattle;
        }
    }
 
    public void AddBuff(HexCell cell)
    {
        GameObject g = Instantiate(WoodSpiritBuff, cell.transform.position, Quaternion.identity);
        CellBuff_Base buff = g.GetComponent<CellBuff_Base>();
        buff.Turns = Turns;
        buff.OnCreated(cell, Owner);
    }

}
