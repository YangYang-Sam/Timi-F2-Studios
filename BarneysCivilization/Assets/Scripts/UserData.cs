using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour
{
    public static UserData instance;
    public string UID;
    public string Password;
    public int Camp;
    public int RaceIndex;

    public string[] AllUsers;
    public int[] AllRaces;
    public int[] AllPoses;
    public int[] RandomSeeds;
    public bool isMultiplayerGame;

    public MapData mapData;
 
    public static bool HasLogin=false;

    public bool ReceiveCardPack;
    public int Round;
    public List<int> CardIDList;
    public List<int> PosIDList;
    public List<string> CardUIDList;

    bool textMessage = false;
    string text;
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

        NetTest.CsResManager.LoginResult += CsResManager_LoginResult;
    }

    private void CsResManager_LoginResult(int obj)
    {
        text = "Login Result: " + obj;
        textMessage = true;
    }
    private void Update()
    {
        if (textMessage)
        {
            textMessage = false;
        }
    }
}
