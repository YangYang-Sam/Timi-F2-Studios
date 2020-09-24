using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ChooseRaceButton : MonoBehaviour
{
    public int RaceIndex;
    public void Choose()
    {
        PlayerController.instance.cardManager.ChooseRace(RaceIndex);
        InGameManager.instance.GameStartProcess();
    }
}
