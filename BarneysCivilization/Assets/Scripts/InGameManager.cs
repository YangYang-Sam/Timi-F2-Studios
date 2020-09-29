using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    public static InGameManager instance;
    public static GameStateType CurrentGameState=GameStateType.Decision;
    public Color[] CampColor;
    public List<CardManager> CardManagers;

    public bool IsTurnEnd;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        NetTest.CsResManager.RoundEndEvent += OnReceiveTurnEnd;
        StartCoroutine(WaitForGameStart());
    }
    private void Update()
    {
        if (IsTurnEnd)
        {
            OnTurnEnd();
        }
    }
    private void OnReceiveTurnEnd()
    {
        IsTurnEnd = true;
    }
    private void OnTurnEnd()
    {
        IsTurnEnd = false;
        foreach (CardManager cm in CardManagers)
        {
            for (int i = 0; i < UserData.instance.AllUsers.Length; i++)
            {
                if (cm.UID == UserData.instance.AllUsers[i])
                {
                    int index = UserData.instance.AllPoses[i];
                    if (index >= 0)
                    {
                        HexCell cell = HexGrid.instance.cells[UserData.instance.AllPoses[i]];
                        cm.SetUnitMoveTo(cell);
                        print("User:" + UserData.instance.AllUsers[i] + " move to cell :" + cell.HexIndex);
                    }       
                    break;
                }
            }
        }
        StartMoving();
    }
    private IEnumerator WaitForGameStart()
    {
        yield return new WaitForSeconds(1);
        GameStartProcess();
    }

    
    public void GameStartProcess()
    {
        UserData data = UserData.instance;
        if (data.isMultiplayerGame)
        {
            for (int i = 0; i < CardManagers.Count; i++)
            {
                if (data.AllRaces.Length > i)
                {
                    CardManagers[i].UID = data.AllUsers[i];
                    CardManagers[i].ChooseRace(data.AllRaces[i]);
                }
                else
                {
                    CardManagers[i].ChooseRace(0);
                }
                if (data.AllUsers[i] == data.UID)
                {
                    PlayerController.instance.cardManager = CardManagers[i];
                }
                else
                {
                    CardManagers[i].gameObject.AddComponent<RemotePlayer>();
                }
            }
        }
        PlayerController.instance.cardManager.ChooseRace(UserData.instance.RaceIndex);

        foreach (CardManager c in CardManagers)
        {
            c.GameStart();
        }
        ChangeGameState(GameStateType.Decision);
    }

    public static bool isGameState(GameStateType type)
    {
        return type == CurrentGameState;
    }
    public void EndTurn()
    {
        if (CurrentGameState == GameStateType.Decision)
        {
            NetTest.NetManager.instance.ReqEndTurn(UserData.instance.UID);
        }
    }

    private void ChangeGameState(GameStateType newState)
    {
        CurrentGameState = newState;
        if (GameStateChangeEvent != null)
        {
            GameStateChangeEvent();
        }
    }

    public void StartMoving()
    {
        StartCoroutine(MoveProcess());
    }

    public void EndBattle()
    {
        if (BeforeTurnEndEvent != null)
        {
            BeforeTurnEndEvent();
        }

        foreach (HexCell cell in HexGrid.instance.cells)
        {
            cell.CheckOwner();
        }       

        if (CurrentGameState == GameStateType.AfterBattle)
        {
            ChangeGameState(GameStateType.Decision);
        }
        if (LateDecisionEvent != null)
        {
            LateDecisionEvent();
        }
    }
    private IEnumerator MoveProcess()
    {
        ChangeGameState(GameStateType.BeforeMove);
        yield return new WaitForSeconds(0.1f);
        ChangeGameState(GameStateType.Move);
        yield return new WaitForSeconds(2.5f);
        ChangeGameState(GameStateType.BeforeBattle);
        yield return new WaitForSeconds(0.1f);
        ChangeGameState(GameStateType.Battle);
        yield return new WaitForSeconds(0.1f);
        ChangeGameState(GameStateType.AfterBattle);
        EndBattle();    
    }
    public void CampLost(CardManager lostManager)
    {
        CardManagers.Remove(lostManager);
        if (lostManager == PlayerController.instance.cardManager)
        {
            PlayerLost();
        }
        if (CardManagers.Count == 1 && CardManagers[0]==PlayerController.instance.cardManager)
        {
            PlayerWin();
        }
    }
    public void PlayerLost()
    {
        UIManager.instance.BattleEnd(false);
    }
    public void PlayerWin()
    {
        UIManager.instance.BattleEnd(true);
    }
    public event System.Action GameStateChangeEvent;
    public event System.Action LateDecisionEvent;
    public event System.Action BeforeTurnEndEvent;
}
public enum GameStateType
{
    Menu,
    Decision,
    BeforeMove,
    Move,
    BeforeBattle,
    Battle,
    AfterBattle
}
