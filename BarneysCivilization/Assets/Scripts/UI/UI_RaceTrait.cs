using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_RaceTrait : MonoBehaviour
{
    public Text RaceNameText;
    public Text RaceElementText;
    public Text TraitNameText;
    public Text TraitDescriptionText;
    public Transform CellIconHolder;

  
    public GameObject[] CellIcons;

    public static UI_RaceTrait instance;
    private void Awake()
    {
        instance = this;
    }
    public void UpdateRaceInfo(RaceInfo info)
    {
        if (RaceNameText != null)
        {
            RaceNameText.text = info.Name;
        }
        if (RaceElementText != null)
        {
            RaceElementText.text = info.Element;
        }
        TraitNameText.text = info.TraitName;
        TraitDescriptionText.text = info.TraitIntroduce;
        for (int i = 0; i < CellIcons.Length; i++)
        {
            Destroy(CellIcons[i]);
        }
        CellIcons = new GameObject[info.FitCells.Length];
        for (int i = 0; i < info.FitCells.Length; i++)
        {
            GameObject icon = Instantiate(ArtResourceManager.instance.GetCellIconPrefab(info.FitCells[i]),CellIconHolder);
            CellIcons[i] = icon;
        }
 
    }
}
