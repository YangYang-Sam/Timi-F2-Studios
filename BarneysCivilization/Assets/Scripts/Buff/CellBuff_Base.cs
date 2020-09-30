using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBuff_Base:MonoBehaviour
{
    public HexCell Cell;
    public CardManager Creator;
    public int Turns;
    public bool Stackable = false;
    public CellBuffType BuffType = CellBuffType.Null;

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

    protected void UpdateBuffType(CellBuffType type, bool forceUpdateType = false)
    {
        if(forceUpdateType)
        {
            BuffType = type;
        }
        else
        {
            if(BuffType == CellBuffType.Null)
            {
                BuffType = type;
            }
        }
    }
}

public enum CellBuffType
{
    Null,
    Cold,
    Fire,
    Rain,
    SandStorm
}