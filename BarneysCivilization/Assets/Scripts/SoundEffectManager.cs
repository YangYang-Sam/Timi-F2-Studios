using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public GameObject[] SoundPrefabs;
    private Transform CameraTransform;
    private void Start()
    {
        CameraTransform = Camera.main.transform;
    }
    public void CreateSoundEffect(int i,float duration)
    {
        GameObject g = Instantiate(SoundPrefabs[i], CameraTransform);
        Destroy(g,duration);
    }
}
