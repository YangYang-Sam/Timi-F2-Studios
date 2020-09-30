using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_CreateBuilding : CardEffect
{
    public bool SupportAllCellTypes = true;
    public List<HexCellType> SupportCellTypes = new List<HexCellType>();
    public int RequireHealth;
    public int DecreaseHealth;
    public GameObject BuildingPrefab;

    public override bool CanUseCard(CardManager user, HexCell cell)
    {
        bool isSupportCellType = SupportAllCellTypes ? true : SupportCellTypes.Contains(cell.CellType);
        return base.CanUseCard(user, cell) && GetCanUseCells(user).Contains(cell) && isSupportCellType;
    }

    public override List<HexCell> GetCanUseCells(CardManager user)
    {
        List<HexCell> cells = new List<HexCell>();
        foreach (HexCell cell in user.OccupiedCells)
        {
            bool isSupportCellType = SupportAllCellTypes ? true : SupportCellTypes.Contains(cell.CellType);
            if (cell.GetUnitOnCell().Health >= RequireHealth && cell.PlacedBuilding == null && isSupportCellType)
            {
                cells.Add(cell);
            }
        }
        return cells;
    }

    public override void Effect(CardManager user, HexCell cell)
    {
        base.Effect(user, cell);
        if (DecreaseHealth != 0)
        {
            cell.GetUnitOnCell().ChangeHealth(-DecreaseHealth);
        }
        user.CreateBuilding(BuildingPrefab, cell);
    }
}
