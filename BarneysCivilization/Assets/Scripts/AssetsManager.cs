using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsManager : MonoBehaviour
{
    public static AssetsManager instance;
    private void Awake()
    {
        instance = this;
    }

    public GameObject[] HexTerrainPrefab;
}
