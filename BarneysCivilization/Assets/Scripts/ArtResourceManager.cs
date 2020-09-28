using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtResourceManager : MonoBehaviour
{
    public static ArtResourceManager instance;

    private void Awake()
    {
        instance = this;
    }

    public GameObject HealEffect;
    public GameObject UpgradeEffect;
    public GameObject BattleEffect;

    public RaceInfo[] RaceInfos;
    public GameObject[] EffectPrefabs;
    public void CreateHealEffect(Vector3 pos)
    {
        GameObject g = Instantiate(HealEffect, pos, Quaternion.identity);
        Destroy(g, 1);
    }
    public void CreateUpgradeEffect(Vector3 pos)
    {
        GameObject g = Instantiate(UpgradeEffect, pos, Quaternion.identity); 
        Destroy(g, 1);
    }
    public void CreateBattleEffect(Vector3 pos)
    {
        GameObject g = Instantiate(BattleEffect, pos, Quaternion.identity); 
        Destroy(g, 1);
    }

    public void CreateEffectByIndex(Vector3 pos,int i,float duration=1)
    {
        GameObject g = Instantiate(EffectPrefabs[i], pos, Quaternion.identity);
        Destroy(g, duration);
    }

    public static string ConverCellTypeText(HexCellType type)
    {
        switch (type)
        {
            case HexCellType.Desert:
                return "沙漠";
            case HexCellType.Forest:
                return "森林";
            case HexCellType.Grass:
                return "草原";
            case HexCellType.Hill:
                return "丘陵";
            case HexCellType.Montain:
                return "山脉";
            case HexCellType.Snow:
                return "雪原";
            case HexCellType.Water:
                return "湖泊";
            default: return "";
        }
    }
}
