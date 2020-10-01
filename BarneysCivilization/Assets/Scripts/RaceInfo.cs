using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RaceInfo
{
    [Header("Introduce")]
    public string Name;
    public string TraitName;
    public string TraitIntroduce;
    public HexCellType[] FitCells;
    public GameObject GalleryPrefab;

    [Header("Prefabs")]
    public GameObject CorePrefab;
    public GameObject UnitPrefab;
    public HexCards[] cards;
    public GameObject RaceTraitPrefab;

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
