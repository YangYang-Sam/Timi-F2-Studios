using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_SnowTreasure : CardEffect
{
    public int amount;
    public override List<HexCell> GetCanUseCells(CardManager user)
    {
        List<HexCell> BuildingGrids = new List<HexCell>();
        foreach (HexCell cell in user.OccupiedCells)
        {
            if (cell.PlacedBuilding != null)
            {
                BuildingGrids.Add(cell);
            }
        }
        return BuildingGrids;
    }

    public override void Effect(CardManager user, HexCell cell)
    {
        base.Effect(user, cell);
        List<HexCell> BuildingGrids = new List<HexCell>();
        foreach (HexCell c in user.OccupiedCells)
        {
            if (c.PlacedBuilding != null)
            {
                BuildingGrids.Add(c);
            }
        }
        foreach(HexCell c in BuildingGrids)
        {
            c.GetUnitOnCell().ChangeHealth(amount, user.GetCorePosition());
        }
    }
}
