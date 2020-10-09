using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public CardManager cardManager;

    public HexCell SelectCell;
    public Unit_Base SelectUnit;

    public PointerJobType jobType;
    public static bool isInBattle;
    public static bool canControl = false;
    public int camp;

    public GameObject MoveIndicator;

    public GameObject[] OrbPrefabs;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        camp = UserData.instance.Camp;
    }

    private void Start()
    {
        if (InGameManager.instance != null)
        {
            InGameManager.instance.GameStateChangeEvent += OnGameStateChange;
        }
    }

    private void OnGameStateChange()
    {
        switch (InGameManager.CurrentGameState)
        {
            case GameStateType.BeforeBattle:
                MoveIndicator.SetActive(false);
                break;
        }        
    }

    private void HexTrace()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            SelectCell = HexGrid.instance.GetCellByPosition(hit.point);               
        }
        else
        {
            if (SelectCell != null)
            {
                SelectCell = null;
            }     
        }        
    }
    public event System.Action<HexCell> PlayerMoveEvent;
    public void PlayerClickOnMap()
    {
        if (canControl)
        {
            HexTrace();
            if (SelectCell != null && SelectCell.isValidCell)
            {
                if (!UserData.instance.isMultiplayerGame || NetTest.NetManager.socket.Connected)
                {
                    if (cardManager.SetUnitMoveTo(SelectCell))
                    {
                        MoveIndicator.SetActive(true);
                        MoveIndicator.transform.position = SelectCell.transform.position;
                        SoundEffectManager.instance.CreateSoundEffect(0);
                    }
                    else
                    {
                        MoveIndicator.SetActive(false);
                    }

                    if (UserData.instance.isMultiplayerGame)
                    {
                        NetTest.NetManager.instance.ReqSetDestiny(UserData.instance.UID, SelectCell.HexIndex);
                        print("Player " + UserData.instance.UID + " Move to: " + SelectCell.HexIndex);
                    }

                    if (PlayerMoveEvent != null)
                    {
                        PlayerMoveEvent(SelectCell);
                    }
                }
                else
                {
                    UI_Warning.instance.ShowWarningText("断线重连中，请稍后操作");
                }
            }
            else
            {
                if (!UserData.instance.isMultiplayerGame || NetTest.NetManager.socket.Connected)
                {
                    cardManager.CancelAllUnitMove();

                    if (UserData.instance.isMultiplayerGame)
                    {
                        print("Player " + UserData.instance.UID + " cancel ");
                        NetTest.NetManager.instance.ReqSetDestiny(UserData.instance.UID, -2);
                    }
                    MoveIndicator.SetActive(false);
                }
                else
                {
                    UI_Warning.instance.ShowWarningText("断线重连中，请稍后操作");
                }
            }
        }
    }
}

public enum PointerJobType
{
    none,
    UseCard,
}