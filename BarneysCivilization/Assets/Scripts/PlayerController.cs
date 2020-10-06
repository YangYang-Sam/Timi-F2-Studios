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
 
    public void PlayerClickOnMap()
    {
        if (canControl)
        {
            HexTrace();
            if (SelectCell != null && SelectCell.isValidCell)
            {
                if(cardManager.SetUnitMoveTo(SelectCell))
                {
                    MoveIndicator.SetActive(true);
                    MoveIndicator.transform.position = SelectCell.transform.position;
                }
                else
                {
                    MoveIndicator.SetActive(false);
                }

                if (UserData.instance.isMultiplayerGame)
                {
                    NetTest.NetManager.instance.ReqSetDestiny(UserData.instance.UID, SelectCell.HexIndex);
                }
            }
            else
            {
                cardManager.CancelAllUnitMove();
                if (UserData.instance.isMultiplayerGame)
                {
                    NetTest.NetManager.instance.ReqSetDestiny(UserData.instance.UID, -2);
                }
                MoveIndicator.SetActive(false);
            }
        }
    }
}

public enum PointerJobType
{
    none,
    UseCard,
}