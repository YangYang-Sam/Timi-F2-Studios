using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public GameObject SoundPlayer;
    public AudioClip[] SoundPrefabs;
    private Transform CameraTransform;
    public static SoundEffectManager instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        CameraTransform = Camera.main.transform;
    }
    public void CreateSoundEffect(int i)
    {
        GameObject g = Instantiate(SoundPlayer, CameraTransform);
        g.GetComponent<AudioSource>().clip = SoundPrefabs[i];
        g.GetComponent<AudioSource>().Play();
       Destroy(g, SoundPrefabs[i].length);
    }
}
