using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCamera : MonoBehaviour
{
    public Vector3 Offset;
    public float Speed;
    private void Start()
    {
        StartCoroutine(StartCamera());
    }

    private IEnumerator StartCamera()
    {
        yield return new WaitForSeconds(1);
        Vector3 position = PlayerController.instance.cardManager.GetCorePosition() + Offset;
        float timer = 1;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, position, Speed);
            yield return null;
        }
    }
}
