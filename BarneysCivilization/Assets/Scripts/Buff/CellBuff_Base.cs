using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBuff_Base : MonoBehaviour
{
    public HexCell Cell;
    public CardManager Creator;
    public int Turns;

    public virtual void OnCreated(HexCell cell, CardManager creator)
    {
        Cell = cell;
        Creator = creator;
        cell.CellBuffs.Add(this);
        InGameManager.instance.GameStateChangeEvent += OnGameStateChange;
    }

    protected virtual void OnGameStateChange()
    {
        if (InGameManager.isGameState(GameStateType.Decision))
        {
            Turns--;
            if (Turns <= 0)
            {
                OnBuffDestroy();
                Destroy(gameObject);
            }
        }
    }

    public virtual void OnBuffDestroy()
    {
        InGameManager.instance.GameStateChangeEvent -= OnGameStateChange;
        Cell.PlacedBuilding = null;
    }
}
