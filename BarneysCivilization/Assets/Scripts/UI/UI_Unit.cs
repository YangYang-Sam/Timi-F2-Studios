using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Unit : MonoBehaviour
{
    public Text HealthText;
    public Text TempHealthText;
    public Unit_Base Unit;
    public Image CampIndicator;

    private void Start()
    {
        CampIndicator.color = InGameManager.instance.CampColor[Unit.Owner.camp];
        Unit.HealthChangeEvent += OnHealthChange;
        Unit.TempHealthChangeEvent += OnTempHealthChange;
        OnTempHealthChange();
        OnHealthChange();
    }

    private void OnTempHealthChange()
    {
        if (Unit.TempHealth != 0)
        {
            TempHealthText.text = Unit.TempHealth.ToString();
        }
        else
        {
            TempHealthText.text = "";
        }
    }

    private void OnHealthChange()
    {
        HealthText.text = Unit.Health.ToString();
    }

}
