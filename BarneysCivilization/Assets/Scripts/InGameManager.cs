using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    public static InGameManager instance;
    public static GameStateType CurrentGameState=GameStateType.Decision;
    public static int TurnCount;
    public Color[] CampColor;
    public List<CardManager> CardManagers= new List<CardManager>();
    public GameObject CardManagerPrefab;

    public float DecisionDuration = 15f;
    private float DecisionTimer;

    public bool IsTurnEnd;

    private List<int> CardIDList = new List<int>();
    private List<int> PosList = new List<int>();
    private List<string> CardUIDList = new List<string>();

    private void Awake()
    {
        instance = this;

        for (int i = 0; i < UserData.instance.mapData.StartCells.Length; i++)
        {
            GameObject g = Instantiate(CardManagerPrefab);
            CardManager cm = g.GetComponent<CardManager>();
            cm.camp = i;
            cm.StartCell = UserData.instance.mapData.StartCells[i];
            CardManagers.Add(cm);
        }
    }
    private void Start()
    {
        NetTest.CsResManager.RoundEndEvent += OnReceiveTurnEnd;
        NetTest.CsResManager.UseCardEvent += OnReceiveUseCard;
        StartCoroutine(WaitForGameStart());
    }
    public void OnReceiveUseCard(string uid, int cardID, int pos)
    {
        CardUIDList.Add(uid);
        PosList.Add(pos);
        CardIDList.Add(cardID);
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

        UserData ud= UserData.instance;
        if (ud.ReceiveCardPack)
        {
            for (int i = 0; i < CardIDList.Count; i++)
            {
                if(CardIDList[i]== ud.CardIDList[i] && CardUIDList[i]==ud.CardUIDList[i] && PosList[i] == ud.PosIDList[i])
                {

                }
                else
                {
                    Debug.LogError("Server: " + ud.CardUIDList[i] + " card: " + ud.CardIDList[i] + " pos:" + ud.PosIDList[i] + " Client:" + CardUIDList[i] + " card: " + CardIDList[i] + " pos:" + PosList[i]);
                }
            }
            if (ud.CardIDList.Count > CardIDList.Count)
            {
                for (int i = CardIDList.Count; i < ud.CardIDList.Count; i++)
                {
                    if (ud.UID != ud.CardUIDList[i])
                    {
                        foreach (CardManager cm in CardManagers)
                        {
                            if (cm.UID == ud.CardUIDList[i])
                            {
                                cm.GetComponent<RemotePlayer>().RemoteUseCard(ud.CardUIDList[i], ud.CardIDList[i], ud.PosIDList[i]);
                            }
                        }
                    }
                }
            }
        }
    }
    private void OnReceiveTurnEnd()
    {
        IsTurnEnd = true;
    }
    private void OnTurnEnd()
    {
        IsTurnEnd = false;
        TurnCount++;
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
                CardManagers[i].ChooseRace(Random.Range(0, 6));
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
        if (LateDecisionEvent != null)
        {
            LateDecisionEvent();
        }
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
                CardUIDList.Clear();
                PosList.Clear();
                CardIDList.Clear();
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
        foreach (HexCell cell in HexGrid.instance.cells)
        {
            cell.CheckOwner();
        }

        ChangeGameState(GameStateType.Move);
        yield return new WaitForSeconds(2.5f);
        foreach (HexCell cell in HexGrid.instance.cells)
        {
            cell.CheckOwner();
        }

        ChangeGameState(GameStateType.BeforeBattle);
        foreach (HexCell cell in HexGrid.instance.cells)
        {
            cell.BeforeBattle();
        }   
        yield return new WaitForSeconds(0.3f);
        foreach (HexCell cell in HexGrid.instance.cells)
        {
            cell.CheckOwner();
        }

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
        foreach (HexCell cell in HexGrid.instance.cells)
        {
            cell.CheckOwner();
        }

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
