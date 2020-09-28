using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BattleEndPannel : MonoBehaviour
{
    public GameObject winPannel;
    public GameObject losePannel;

    public void ShowEndPannel(bool isWin)
    {
        if (isWin)
        {
            winPannel.SetActive(true);
        }
        else
        {
            losePannel.SetActive(true);
        }
    }

    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
