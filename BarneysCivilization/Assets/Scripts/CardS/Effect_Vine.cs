using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Vine : CardEffect
{
    public int ConsumeHealth;
    public int Turns = 1;
    public GameObject BuffPrefab;
    public List<HexCellType> SupportCellTypes = new List<HexCellType>();

    public bool IsSupportCellType(CardManager user, HexCell cell)
    {
        if (cell && (cell.OwnerManager == user))
        {
            if(SupportCellTypes.Contains(cell.CellType))
            {
                if (cell.GetUnitOnCell().Health > ConsumeHealth)
                {
                    return true;
                }                
            }
        }

        return false;
    }

    public override bool CanUseCard(CardManager user, HexCell cell)
    {
        return base.CanUseCard(user, cell);
    }

    public override List<HexCell> GetCanUseCells(CardManager user)
    {
        List<HexCell> cells = new List<HexCell>();

        foreach (var cell in user.OccupiedCells)
        {
            if(IsSupportCellType(user, cell))
            {
                cells.Add(cell);
            }        
        }

        return cells;
    }

    public override void Effect(CardManager user, HexCell cell)
    {
        cell.GetUnitOnCell().TakeDamage(ConsumeHealth,null);
        GameObject g = Instantiate(BuffPrefab, cell.transform.position, Quaternion.identity);
        CellBuff_Base buff = g.GetComponent<CellBuff_Base>();
        buff.Turns = Turns;
        buff.OnCreated(cell, user);
    }
}
