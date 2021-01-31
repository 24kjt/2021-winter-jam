using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundEffectController : MonoBehaviour
{
    public AudioSource audiosource;

    public AudioClip bonk;
    public AudioClip moveWoosh;
    public AudioClip win;

    void playBonk(){
        audiosource.clip = bonk;
    }

    void playWoosh(){
        audiosource.clip = moveWoosh;
    }

    void playWin(){
        audiosource.clip = win;
    }
}
