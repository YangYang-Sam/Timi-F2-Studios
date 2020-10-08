using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    public GameObject[] Prefabs;
    private int Index;
    public bool isInstanceGoing;
    private void Awake()
    {
        if (UserData.instance.UseTutorial)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        UIManager.instance.StartChooseCardEvent += OnChooseCard;
    }

    private void OnChooseCard()
    {
        StartCoroutine(TutorialProcess());
    }

    private void OnGameStart()
    {
        StartCoroutine(TutorialProcess());
    }

    IEnumerator TutorialProcess()
    {        
        UIManager.instance.ChooseRandomCard();
        while (Index < Prefabs.Length)
        {
            GameObject g = Instantiate(Prefabs[Index]);
            isInstanceGoing = true;

            while (isInstanceGoing)
            {
                yield return null;
            }

        }
        yield return null;
        //PlayerController.instance.cardManager.random
    }
   
}
