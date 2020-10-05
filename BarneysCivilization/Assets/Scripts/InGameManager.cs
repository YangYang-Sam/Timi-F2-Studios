using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    public static InGameManager instance;
    public static GameStateType CurrentGameState=GameStateType.Decision;
    public Color[] CampColor;
    public List<CardManager> CardManagers;

    public float DecisionDuration = 15f;
    private float DecisionTimer;

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
        if(Time.time> DecisionTimer && PlayerController.canControl)
        {
            EndTurn();
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
        else
        {
            PlayerController.instance.cardManager = CardManagers[0];
            CardManagers[0].ChooseRace(UserData.instance.RaceIndex);
            for (int i = 1; i < CardManagers.Count; i++)
            {
                CardManagers[i].ChooseRace(Random.Range(0, 2));
                CardManagers[i].gameObject.AddComponent<AIController>();
            }
        }
        PlayerController.instance.cardManager.ChooseRace(UserData.instance.RaceIndex);
        UI_RaceTrait.instance.UpdateRaceInfo(ArtResourceManager.instance.RaceInfos[data.RaceIndex]); 

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
        if (PlayerController.canControl)
        {
            PlayerController.canControl = false;
            if (UserData.instance.isMultiplayerGame)
            {
                NetTest.NetManager.instance.ReqEndTurn(UserData.instance.UID);
            }
            else
            {
                StartMoving();
            }
        }
    }

    private void ChangeGameState(GameStateType newState)
    {
        CurrentGameState = newState;
        if (GameStateChangeEvent != null)
        {
            GameStateChangeEvent();
        }
        switch (newState)
        {
            case GameStateType.Decision:
                PlayerController.canControl = true;
                DecisionTimer = Time.time + DecisionDuration;
                if (!UserData.instance.isMultiplayerGame)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        UserData.instance.RandomSeeds[i] = Random.Range(0, int.MaxValue);
                    }
                }
                break;
        }
    }

    public void StartMoving()
    {
        StartCoroutine(MoveProcess());
    }

    private IEnumerator MoveProcess()
    {
        ChangeGameState(GameStateType.BeforeMove);
        yield return new WaitForSeconds(0.1f);

        ChangeGameState(GameStateType.Move);
        yield return new WaitForSeconds(2.5f);

        ChangeGameState(GameStateType.BeforeBattle);
        foreach (HexCell cell in HexGrid.instance.cells)
        {
            cell.BeforeBattle();
        }   
        yield return new WaitForSeconds(0.1f);
  
        ChangeGameState(GameStateType.Battle);
        foreach (HexCell cell in HexGrid.instance.cells)
        {
            cell.Battle();
        }
        yield return new WaitForSeconds(0.1f);

        ChangeGameState(GameStateType.AfterBattle);
        foreach (HexCell cell in HexGrid.instance.cells)
        {
            cell.AfterBattle();
        }
        if (BeforeTurnEndEvent != null)
        {
            BeforeTurnEndEvent();
        }
        yield return new WaitForSeconds(0.1f);

        ChangeGameState(GameStateType.Decision);
        if (LateDecisionEvent != null)
        {
            LateDecisionEvent();
        }
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
        if (UserData.instance.isMultiplayerGame)
        {
            NetTest.NetManager.instance.ReqGameEnd(UserData.instance.UID);
        }
    }
    public void PlayerWin()
    {
        UIManager.instance.BattleEnd(true);
        if (UserData.instance.isMultiplayerGame)
        {
            NetTest.NetManager.instance.ReqGameEnd(UserData.instance.UID);
        }
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
