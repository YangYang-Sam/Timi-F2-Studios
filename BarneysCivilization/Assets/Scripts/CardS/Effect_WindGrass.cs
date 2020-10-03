using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_WindGrass : CardEffect
{
    List<HexCell> EmptyDessertCells=new List<HexCell>();

    public override bool CanUseCard(CardManager user, HexCell cell)
    {
        UpdateDesertCell(user);
        return base.CanUseCard(user, cell) && EmptyDessertCells.Count > 0;
    }
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
        UpdateDesertCell(user);

        Building_Base building = cell.PlacedBuilding;
        UpdateDesertCell(user);
        Random.InitState(UserData.instance.RandomSeeds[2]);
        HexCell randomCell = EmptyDessertCells[Random.Range(0, EmptyDessertCells.Count)];

        building.ChangeCell(randomCell);
   }

    public void UpdateDesertCell(CardManager user)
    {
        EmptyDessertCells.Clear();
        foreach(HexCell cell in user.OccupiedCells)
        {
            if(cell.CellType== HexCellType.Desert && cell.PlacedBuilding==null)
            {
                EmptyDessertCells.Add(cell);
            }
        }
    }
}
