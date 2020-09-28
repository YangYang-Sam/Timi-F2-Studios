using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RaceInfo
{
    public string Name;
    public GameObject CorePrefab;
    public GameObject UnitPrefab;
    public HexCards[] cards;


    [System.Serializable]
    public class HexCards
    {
        public GameObject[] cards;
    }
}
public enum RaceType
{
    Deer,
    Wolf
}
