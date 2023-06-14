using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseLevel : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject lines;

    public void onClickPause()
    {
        pauseMenu.SetActive(true);
        lines.SetActive(false);
    }

    public void onClickContinue()
    {
        pauseMenu.SetActive(false);
        lines.SetActive(true);
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
