using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using NetTest;
using UnityEngine.UI;

public class UI_MainScene : MonoBehaviour
{
    public static UI_MainScene instance;
  
    Animator animator;
    Camera mainCam;

    public float ChooseRaceCameraPitch;
    public float MainSceneCameraPitch;

    private bool isMatching;
    private bool MatchFound;

    public GameObject GalleryInstance;
    public UI_RaceTrait RaceTrait;

    public UI_ChooseRaceButton[] RaceButtons;

    public MapData[] MapDatas;
    public SpriteRenderer CameraBackgroundRenderer;
    public Sprite[] RaceBackgrounds;


    public bool cancelMatch=false;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        mainCam = Camera.main;

        CsResManager.MatchingBeginEvent += OnMatchingBegin;
        CsResManager.MatchingErrorEvent += OnMatchingError;
        CsResManager.MatchSuccessEvent += OnMatchSuccess;

        int RaceIndex = Random.Range(0, 6);
        ChooseRace(RaceIndex);
        UserData.instance.RaceIndex = RaceIndex;        
    }
    private void Update()
    {
        if(isMatching  && MatchFound)
        {
            MatchFound = false;           
            SceneManager.LoadScene("BattleScene");
            CsResManager.MatchingErrorEvent -= OnMatchingError;
        }
        if (cancelMatch)
        {
            cancelMatch = false;
            CancelMatch();
        }
    }
    private void OnMatchSuccess(string[] Users, int[] Races, int[] RandomSeeds)
    {
        MatchFound = true;
        UserData data = UserData.instance;
        data.isMultiplayerGame = true;
        data.AllUsers = new string[Users.Length];
        data.AllRaces = new int[Races.Length];
        data.RandomSeeds = new int[RandomSeeds.Length];
        for (int i = 0; i < Users.Length; i++)
        {
            UserData.instance.AllUsers[i] = Users[i];
            if (data.UID == data.AllUsers[i])
            {
                data.Camp = i;
            }
        }
        for (int i = 0; i < Races.Length; i++)
        {
            UserData.instance.AllRaces[i] = Races[i] - 1;
        }
        for (int i = 0; i < RandomSeeds.Length; i++)
        {
            UserData.instance.RandomSeeds[i] = RandomSeeds[i];
        }
        
    }

    private void OnMatchingBegin()
    {
       
    }

    private void OnMatchingError()
    {
        cancelMatch = true;
    }

    public void SinglePlayerStart(int MapIndex)
    {
        UserData.instance.RandomSeeds = new int[10];
        for (int i = 0; i < 10; i++)
        {
            UserData.instance.RandomSeeds[i] = Random.Range(0, int.MaxValue);
        }

        UserData.instance.mapData = MapDatas[MapIndex];
        CsResManager.MatchingErrorEvent -= OnMatchingError;
        SceneManager.LoadScene("BattleScene");
    }
    public void StartMatching(int MapIndex)
    {
        animator.Play("StartMatching");
        isMatching = true;
        UserData.instance.mapData = MapDatas[MapIndex];
        //NetManager.instance.ReLogin();
        NetManager.instance.ReqMatching(UserData.instance.UID, UserData.instance.RaceIndex+1);
    }
    public void StartChooseRace()
    {
        animator.Play("StartChooseRace");
        RaceButtons[UserData.instance.RaceIndex].SetColorDirectly(true);
        if (CameraRotateProcess != null)
        {
            StopCoroutine(CameraRotateProcess);
        }
        CameraRotateProcess = StartCoroutine(CameraRotate(ChooseRaceCameraPitch));
    }
    public void StartMainScene()
    {
        animator.Play("FinishChooseRace");
        if (CameraRotateProcess != null)
        {
            StopCoroutine(CameraRotateProcess);
        }
        CameraRotateProcess = StartCoroutine(CameraRotate(MainSceneCameraPitch));
    }
    public void CancelMatch()
    {
        StartMainScene();
        NetManager.instance.ReqStopMatching(UserData.instance.UID);
    }
    private Coroutine CameraRotateProcess;
    private IEnumerator CameraRotate(float TargetPitch)
    {
        float timer = 1;
        while (timer > 0)
        {
            yield return null;
            timer -= Time.deltaTime;
            Vector3 targetEuler = mainCam.transform.eulerAngles;
            targetEuler.x = TargetPitch;
            mainCam.transform.eulerAngles = Vector3.Lerp(mainCam.transform.eulerAngles, targetEuler, 0.1f);
        }
    }

    public void ChooseRace(int raceIndex)
    {
        RaceInfo info = ArtResourceManager.instance.RaceInfos[raceIndex];

        RaceTrait.UpdateRaceInfo(info);
        if (GalleryInstance != null)
        {
            Destroy(GalleryInstance);
        }
        GalleryInstance = Instantiate(info.GalleryPrefab);
        CameraBackgroundRenderer.sprite = RaceBackgrounds[raceIndex];
    }
}
[System.Serializable]
public class MapData
{
    public int Width;
    public int Radius;
    public Vector3Int CenterGrid;
    public int[] StartCells;
    public bool useTutorial;
}