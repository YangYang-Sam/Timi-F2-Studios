using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using NetTest;

public class UI_MainScene : MonoBehaviour
{
    Animator animator;
    Camera mainCam;

    public float ChooseRaceCameraPitch;
    public float MainSceneCameraPitch;

    private bool isMatching;
    private bool MatchFound;

    private void Start()
    {
        animator = GetComponent<Animator>();
        mainCam = Camera.main;

        CsResManager.MatchingBeginEvent += OnMatchingBegin;
        CsResManager.MatchSuccessEvent += OnMatchSuccess;
    }
    private void Update()
    {
        if(isMatching  && MatchFound)
        {
            MatchFound = false;           
            SceneManager.LoadScene("BattleScene");
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
        print("Match Begin");
    }

    public void SinglePlayerStart()
    {
        UserData.instance.RandomSeeds = new int[10];
        for (int i = 0; i < 10; i++)       
        {
            i = Random.Range(0, int.MaxValue);
        }
        SceneManager.LoadScene("BattleScene");
    }
    public void StartMatching()
    {
        animator.Play("StartMatching");
        isMatching = true;
        NetManager.instance.ReqMatching(UserData.instance.UID,UserData.instance.RaceIndex+1);
    }
    public void StartChooseRace()
    {
        animator.Play("StartChooseRace");

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
}
