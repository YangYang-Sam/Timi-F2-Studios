using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ResourcePannel : MonoBehaviour
{
    public int resource;
    public Text ResourceText;
    public Text ActionPointText;
    public Animator animator;

    private void Update()
    {
        if (PlayerController.instance.cardManager)
        {
            if(resource != PlayerController.instance.cardManager.GetTotalResource())
            {
                resource = PlayerController.instance.cardManager.GetTotalResource();
                animator.Play("ResourceIncrease");
            }        

            ResourceText.text =PlayerController.instance.cardManager.GetTotalResource().ToString();
            ActionPointText.text = PlayerController.instance.cardManager.ActionPoint.ToString();
        }
    }
}
