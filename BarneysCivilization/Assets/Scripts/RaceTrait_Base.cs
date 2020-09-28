using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceTrait_Base : MonoBehaviour
{
    public CardManager owner;
    public void InitRaceTrait(CardManager cardManager)
    {
        owner = cardManager;
    }
}
