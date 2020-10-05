using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
    public static UserData instance;
    public string UID;
    public int Camp;
    public int RaceIndex;

    public string[] AllUsers;
    public int[] AllRaces;
    public int[] AllPoses;
    public int[] RandomSeeds;
    public bool isMultiplayerGame;

    public int PlayerAmount;
 
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
}
