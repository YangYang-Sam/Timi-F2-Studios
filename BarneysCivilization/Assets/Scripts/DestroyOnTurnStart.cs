using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTurnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InGameManager.instance.LateDecisionEvent += OnLateDecision;
    }

    private void OnLateDecision()
    {
        Destroy(gameObject);
    }
}
