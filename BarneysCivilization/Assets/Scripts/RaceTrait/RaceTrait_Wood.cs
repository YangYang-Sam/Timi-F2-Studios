using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceTrait_Wood : RaceTrait_Base
{
    private void Start()
    {
        owner.DrawCardAmount = 2;
        InGameManager.instance.GameStateChangeEvent += Instance_GameStateChangeEvent;
    }

    private void Instance_GameStateChangeEvent()
    {
        if (InGameManager.isGameState(GameStateType.Decision))
        {
            List<HexCell> BuildingGrids = new List<HexCell>();
            foreach(HexCell cell in owner.OccupiedCells)
            {
                if (cell.PlacedBuilding != null)
                {
                    BuildingGrids.Add(cell);
                }
            }
            owner.DrawCards(Mathf.Min(3, BuildingGrids.Count));
        }
    }
}
