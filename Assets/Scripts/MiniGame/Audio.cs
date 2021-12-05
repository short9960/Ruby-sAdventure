using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioClip musicClipOne;
    // Start is called before the first frame update
    void Start()
    {
        musicSource.clip = musicClipOne;
        musicSource.Play();
        musicSource.loop = true;
    }

}
