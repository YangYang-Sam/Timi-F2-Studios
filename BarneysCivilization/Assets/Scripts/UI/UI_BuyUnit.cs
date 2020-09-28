using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BuyUnit : MonoBehaviour
{
    public GameObject UnitPrefab;
    public int ResourceCost;

    [SerializeField]
    private Text ResourceCostText;
    [SerializeField]
    private Text HealthText;

    //private void Awake()
    //{
    //    ResourceCost = UnitPrefab.GetComponent<Unit_Base>().ResourceCost;
    //    ResourceCostText.text = "资源消耗: " + ResourceCost;
    //    HealthText.text = "战斗力: " + UnitPrefab.GetComponent<Unit_Base>().Health;
    //}
    //public void ClickOn()
    //{
    //    if (PlayerController.instance.cardManager.ResourceAmount >= ResourceCost && PlayerController.instance.cardManager.BuyUnitTimes>0)
    //    {
    //        if (PlayerController.instance.jobType == PointerJobType.BuyUnit)
    //        {
    //            PlayerController.instance.CancelBuy();
    //        }
    //        PlayerController.instance.BuyUnitStart(UnitPrefab,ResourceCost);
    //    }
    //}
}
