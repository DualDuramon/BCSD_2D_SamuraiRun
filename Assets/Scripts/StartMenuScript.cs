using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuScript : MonoBehaviour
{
    public GameObject howToScreen;

    public void PlayGameBtn()
    {
        SceneManager.LoadScene("GameStage");
    }

    public void HowToBtn()
    {
        howToScreen.SetActive(true);
    }

    public void HowToBtnQuit()
    {
        howToScreen.SetActive(false);
    }
    
    public void QuitGameBtn()
    {
        Application.Quit();
    }
}
