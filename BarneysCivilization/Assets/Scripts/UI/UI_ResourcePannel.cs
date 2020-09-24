using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ResourcePannel : MonoBehaviour
{
    public Text ResourceText;
    public Text BuyUnitTimesText;
    public Text ActionPointText;

    private void Update()
    {
        ResourceText.text = "资源： " + PlayerController.instance.cardManager.GetTotalResource();
        ActionPointText.text = "行动点： " + PlayerController.instance.cardManager.ActionPoint;
        BuyUnitTimesText.text = "领土数： " + PlayerController.instance.cardManager.OccupiedCells.Count;
    }
}
