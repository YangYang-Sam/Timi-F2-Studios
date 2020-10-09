using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTurnStart : MonoBehaviour
{
    public bool destroyOnGameState;
    public GameStateType type;
    // Start is called before the first frame update
    void Start()
    {
        InGameManager.instance.LateDecisionEvent += OnLateDecision;
        InGameManager.instance.GameStateChangeEvent += OnGameState;
    }

    private void OnGameState()
    {
        if (InGameManager.isGameState(type) && destroyOnGameState)
        {
            DestroyThis();
        }
    }

    private void OnLateDecision()
    {
        if (!destroyOnGameState)
        {
            DestroyThis();
        }
    }
    public void DestroyThis()
    {
        InGameManager.instance.LateDecisionEvent -= OnLateDecision;
        InGameManager.instance.GameStateChangeEvent -= OnGameState;
        Destroy(gameObject);
    }

}
