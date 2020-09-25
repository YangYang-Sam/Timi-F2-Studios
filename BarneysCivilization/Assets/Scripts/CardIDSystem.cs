using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardIDSystem : MonoBehaviour
{
    public static CardIDSystem instance;

    public GameObject[] AllCardObjects;
    public string[] AllCardNames;

    private void Awake()
    {
        instance = this;
        AllCardNames = new string[AllCardObjects.Length];
        for (int i = 0; i < AllCardObjects.Length; i++)
        {
            AllCardNames[i] = AllCardObjects[i].GetComponent<Card_Base>().CardName;
        }
    }

    public int GetCardID(string CardName)
    {
        for (int i = 0; i < AllCardNames.Length; i++)
        {
            if (AllCardNames[i] == CardName)
            {
                return i;
            }
        }
        return -1;
    }
    public GameObject GetCardByID(int ID)
    {
        return AllCardObjects[ID];
    }
}
