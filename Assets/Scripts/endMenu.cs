using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endMenu : MonoBehaviour
{
    int currentScreen = 0;

    public GameObject endscene1;
    public GameObject endscene2;

    void Update(){
        switch(currentScreen) {
            case 0:
                if(Input.anyKeyDown){
                    currentScreen++;
                    endscene1.SetActive(false);
                    endscene2.SetActive(true);
                }
                break;
            case 1:
                if(Input.anyKeyDown){
                    SceneManager.LoadScene(0);
                }
                break;
            default:
                break;
        }
    }
}
