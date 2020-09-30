using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBuff_Base:MonoBehaviour
{
    public HexCell Cell;
    public CardManager Creator;
    public int Turns;
    public bool Stackable=false;
    public CellBuffType BuffType;

    public virtual void OnCreated(HexCell cell, CardManager creator)
    {
        Cell = cell;
        Creator = creator;
        cell.AddBuff(this);
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
            }
        }

    }

    public virtual void OnStack(CellBuff_Base newBuff)
    {
        Turns += newBuff.Turns;
    }

    public virtual void OnBuffDestroy()
    {
        InGameManager.instance.GameStateChangeEvent -= OnGameStateChange;
        Cell.RemoveBuff(this);
        Destroy(gameObject);
    }
}

public enum CellBuffType
{
    Rain,
    Fire
}