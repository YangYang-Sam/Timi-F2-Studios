using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ChooseRaceButton : MonoBehaviour
{
    public int RaceIndex;
    public void Choose()
    {        
        UserData.instance.RaceIndex = RaceIndex;
    }
}
