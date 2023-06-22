using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseLevel : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject lines;
    [SerializeField] private GameObject levelEditorButton;

    public void onClickPause()
    {
        levelEditorButton.GetComponent<Button>().interactable = LevelName.getStoryMode() ? false : true;
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
        LevelName.setMenu(LevelName.getStoryMode() ? "Story" : "Level");
        SceneManager.LoadScene(0);
    }
}
