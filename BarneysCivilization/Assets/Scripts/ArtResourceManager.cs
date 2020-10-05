using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtResourceManager : MonoBehaviour
{
    public static ArtResourceManager instance;
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }        
    }

    public GameObject HealEffect;
    public GameObject UpgradeEffect;
    public GameObject BattleEffect;

    public GameObject TextEffectPrefab;

    public GameObject BallPrefab;

    public RaceInfo[] RaceInfos;
    public GameObject[] EffectPrefabs;
    public GameObject[] CellIconPrefab;


    [System.Serializable]
    public class TerrainPrefabs
    {
        public GameObject[] TerrainPerfab;
    }
    public TerrainPrefabs[] Terrains;
    public GameObject GetTerrainPrefab(int typeIndex)
    {
        return Terrains[typeIndex].TerrainPerfab[Random.Range(0, Terrains[typeIndex].TerrainPerfab.Length)];
    }
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
    public void CreateTextEffect(string Text, Vector3 position)
    {
        CreateTextEffect(Text, position, Color.white, 1);
    }

    public void CreateTextEffect(string Text, Vector3 position,Color c,float sizeMultiplier)
    {
        GameObject g = Instantiate(TextEffectPrefab, position+ Vector3.up, Quaternion.identity);
        Text t = g.GetComponentInChildren<Text>();
        t.text = Text;
        t.color = c;
        g.transform.localScale *= sizeMultiplier;
        Destroy(g, 1.5f);
    }

    public void CreateEffectByIndex(Vector3 pos,int i,float duration=1)
    {
        GameObject g = Instantiate(EffectPrefabs[i], pos, Quaternion.identity);
        Destroy(g, duration);
    }

    public GameObject GetCellIconPrefab(HexCellType type)
    {
        return CellIconPrefab[(int)type];
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
