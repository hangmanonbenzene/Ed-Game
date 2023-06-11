using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseLevel : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    public void onClickPause()
    {
        pauseMenu.SetActive(true);
    }

    public void onClickContinue()
    {
        pauseMenu.SetActive(false);
    }
    public void onClickLevelEditor()
    {
        SceneManager.LoadScene(2);
    }
    public void onClickQuit()
    {
        SceneManager.LoadScene(0);
    }
}
