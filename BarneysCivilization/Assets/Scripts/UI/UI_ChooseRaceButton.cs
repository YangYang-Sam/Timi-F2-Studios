using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Threading;

public class UI_ChooseRaceButton : MonoBehaviour
{
    public int RaceIndex;
    public Image BackImage;
    public float Duration = 0.3f;

    public Color NormalColor;
    public Color HighlightColor;

    public void Choose()
    {        
        UI_MainScene.instance.ChooseRace(RaceIndex);

        UI_MainScene.instance.RaceButtons[UserData.instance.RaceIndex].SetHighlight(false);
        SetHighlight(true);

        UserData.instance.RaceIndex = RaceIndex;
    }
    Coroutine ColorProcess;
    public void SetHighlight(bool isHighlight)
    {
        if (ColorProcess != null)
        {
            StopCoroutine(ColorProcess);
        }

        if (isHighlight)
        {
            ColorProcess = StartCoroutine(SetColor(NormalColor, HighlightColor));
        }
        else
        {
            ColorProcess = StartCoroutine(SetColor(HighlightColor, NormalColor));
        }
    }
    private IEnumerator SetColor(Color startColor,Color targetColor)
    {
        float timer = Duration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            BackImage.color = Color.Lerp(startColor, targetColor, 1 - timer / Duration);
            yield return null;
        }
    }
}