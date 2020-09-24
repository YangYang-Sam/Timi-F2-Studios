using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_CreateBuilding : Card_Base
{
    public int RequireHealth;
    public int DecreaseHealth;
    public GameObject BuildingPrefab;

    public override bool CanUseCard(CardManager user, HexCell cell)
    {
        return base.CanUseCard(user, cell) && GetCanUseCells(user).Contains(cell);
    }
    public override List<HexCell> GetCanUseCells(CardManager user)
    {
        List<HexCell> cells = new List<HexCell>();
        foreach(HexCell cell in user.OccupiedCells)
        {
            if (cell.GetUnitOnCell().Health >= RequireHealth && cell.PlacedBuilding==null)
            {
                cells.Add(cell);
            }
        }
        return cells;
    }
    public override void CardEffect(CardManager user, HexCell cell)
    {
        base.CardEffect(user, cell);
        if (DecreaseHealth != 0)
        {
            cell.GetUnitOnCell().ChangeHealth(-DecreaseHealth);
        }
        user.CreateBuilding(BuildingPrefab, cell);
    }
}
