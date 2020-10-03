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
        DontDestroyOnLoad(gameObject);
    }

    private void HexTrace()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            if (SelectCell != null)
            {
            }
            SelectCell = HexGrid.instance.GetCellByPosition(hit.point);
            if (SelectCell != null)
            {
            }           
        }
        else
        {
            if (SelectCell != null)
            {
                SelectCell = null;
            }     
        }        
    }

    void Update()
    {
        if (cardManager != null && canControl)
        {
            HexTrace();
            if (Input.GetMouseButtonDown(0) && SelectCell != null)
            {
                cardManager.SetUnitMoveTo(SelectCell);
                if (UserData.instance.isMultiplayerGame)
                {
                    NetTest.NetManager.instance.ReqSetDestiny(UserData.instance.UID, SelectCell.HexIndex);
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