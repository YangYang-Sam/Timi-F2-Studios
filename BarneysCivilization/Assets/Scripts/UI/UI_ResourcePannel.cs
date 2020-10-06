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
        if (PlayerController.instance.cardManager)
        {
            ResourceText.text = "资源： " + PlayerController.instance.cardManager.GetTotalResource();
            ActionPointText.text = PlayerController.instance.cardManager.ActionPoint.ToString();
            BuyUnitTimesText.text = "领土数： " + PlayerController.instance.cardManager.OccupiedCells.Count;
        }
    }
}
