using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    int currentScreen = 0;
    bool keyDown = false;

    public GameObject cutscene1;
    public GameObject cutscene2;
    public GameObject instructions2;

    void Update(){
        switch(currentScreen) {
            case 1:
                if(Input.anyKeyDown){
                    currentScreen++;
                    cutscene1.SetActive(false);
                    cutscene2.SetActive(true);
                    keyDown = true;
                }
                break;
            case 2:
                if(Input.anyKeyDown){
                    currentScreen++;
                    cutscene2.SetActive(false);
                    instructions2.SetActive(true);
                    keyDown = true;
                }
                break;
            case 3:
                if (Input.anyKeyDown && !keyDown){
                    PlayGame();
                } 
                if (keyDown && !Input.anyKeyDown) {
                    keyDown = false;
                }
                break;
            default:
                break;
        }
    }
    public void PlayGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayButton(){
        currentScreen++;
        GameObject.Find("Canvas").SetActive(false);
        GameObject.Find("title page").SetActive(false);
        cutscene1.SetActive(true);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
