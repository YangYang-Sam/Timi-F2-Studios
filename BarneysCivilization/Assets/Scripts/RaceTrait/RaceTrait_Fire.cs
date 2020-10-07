using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceTrait_Fire : RaceTrait_Base
{
    public GameObject BuffPrefab;
    public int Turns = 2;
    private void Start()
    {
        owner.OccupyNewCellEvent += OnOccupyNewCell;
    }

    private void OnOccupyNewCell(HexCell cell)
    {
        GameObject g = Instantiate(BuffPrefab, cell.transform.position, Quaternion.identity);
        CellBuff_Base buff = g.GetComponent<CellBuff_Base>();
        buff.Turns = Turns;
        buff.OnCreated(cell, owner);
        if (cell.CellType == HexCellType.Forest || cell.CellType== HexCellType.Grass)
        {
            cell.GetUnitOnCell().ChangeHealth(1, owner.GetCorePosition());
        }
    }
}
