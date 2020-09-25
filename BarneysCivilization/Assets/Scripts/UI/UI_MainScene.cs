using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainScene : MonoBehaviour
{
    Animator animator;
    Camera mainCam;

    public float ChooseRaceCameraPitch;
    public float MainSceneCameraPitch;

    private void Start()
    {
        animator = GetComponent<Animator>();
        mainCam = Camera.main;
    }
    public void SinglePlayerStart()
    {
        SceneManager.LoadScene("BattleScene");
    }
    public void StartMatching()
    {
        animator.Play("StartMatching");
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
