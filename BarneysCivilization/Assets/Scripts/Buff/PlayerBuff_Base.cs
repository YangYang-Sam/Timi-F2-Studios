using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuff_Base : MonoBehaviour
{
    public CardManager Owner;
    public CardManager Creator;
    public int Turns;
    public bool Stackable = false;
    public PlayerBuffType BuffType = PlayerBuffType.Null;

    public virtual void OnCreated(CardManager owner, CardManager creator)
    {
        Owner = owner;
        Creator = creator;
        owner.AddBuff(this);
        InGameManager.instance.GameStateChangeEvent += OnGameStateChange;
        owner.UnitBeforeBattleEvent += OnUnitBeforeBattle;
        owner.UnitWinBattleEvent += OnUnitWinBattle;
    }

    protected virtual void OnUnitWinBattle(Unit_Base unit, HexCell cell)
    {

    }

    protected virtual void OnUnitBeforeBattle(Unit_Base unit, HexCell cell)
    {

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

    public virtual void OnStack(PlayerBuff_Base newBuff)
    {
        Turns += newBuff.Turns;
    }

    public virtual void OnBuffDestroy()
    {
        InGameManager.instance.GameStateChangeEvent -= OnGameStateChange;
        Owner.UnitBeforeBattleEvent -= OnUnitBeforeBattle;
        Owner.UnitWinBattleEvent -= OnUnitWinBattle;
        Owner.RemoveBuff(this);
        Destroy(gameObject);
    }

}

public enum PlayerBuffType
{
    Null,
    WaterGather,
    Flood,
    BurnToGround,
    Revive,
    EscapeElectric,
    Superconduct,
    Thunder,
    FierceWind,
    RollingStone
}