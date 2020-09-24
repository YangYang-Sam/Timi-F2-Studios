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

    public int camp;

    private void Awake()
    {
        instance = this;
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
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    cardManager.DrawCard();
        //}

        //switch (jobType)
        //{
        //    case PointerJobType.BuyUnit:
        //        if(SelectUnit!=null && SelectCell != null)
        //        {
        //            SelectUnit.transform.position = SelectCell.transform.position;
        //            if (Input.GetMouseButtonDown(0) && SelectCell.CanPlaceUnit(cardManager))
        //            {
        //                cardManager.BuyUnitOnCell(SelectUnit, SelectCell, ResourceCost);
        //                SelectUnit = null;
        //                jobType = PointerJobType.none;
        //            }                 
        //        }

        //        break;
        //}
        HexTrace();
        if (Input.GetMouseButtonDown(0) && SelectCell!=null)
        {
            cardManager.SetUnitMoveTo(SelectCell);
        }       
    } 


}

public enum PointerJobType
{
    none,
    UseCard,
}