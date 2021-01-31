using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundEffectController : MonoBehaviour
{
    public AudioSource audiosource;

    public AudioClip bonk;
    public AudioClip moveWoosh;
    public AudioClip win;

    bool bonkDone = false;
    bool winDone = false;
    bool wooshDone = false;

    // public void playBonk() {
    //     StartCoroutine(_playBonk());
    // }

    // public void playWoosh() {
    //     StartCoroutine(_playWoosh());
    // }

    // public void playWin() {
    //     StartCoroutine(_playWin());
    // }

    public IEnumerator playBonk(){
        audiosource.clip = bonk;
        audiosource.Play();
        
        while (audiosource.isPlaying)
            yield return null;
    }

    public void playWoosh(){
        audiosource.clip = moveWoosh;
        audiosource.Play();

        // while (audiosource.isPlaying)
        //     yield return null;
    }

    public IEnumerator playWin(){
        audiosource.clip = win;
        audiosource.Play();
        
        while (audiosource.isPlaying)
            yield return null;
    }
}
