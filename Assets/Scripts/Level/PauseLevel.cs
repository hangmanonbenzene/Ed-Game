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
    [SerializeField] private GameObject continueButton;

    private bool pauseButtonactive = true;

    public void onClickPause()
    {
        setPauseButtonActive(false);
        levelEditorButton.GetComponent<Button>().interactable = LevelName.getStoryMode() ? false : true;
        pauseMenu.SetActive(true);
        lines.SetActive(false);
        continueButton.GetComponent<Button>().Select();
    }

    public void onClickContinue()
    {
        setPauseButtonActive(true);
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

    public void setPauseButtonActive(bool active)
    {
        pauseButtonactive = active;
    }

    private void Update()
    {
        if (pauseButtonactive)
        {
            if (Input.GetKeyDown("joystick button 1") || Input.GetKeyDown(KeyCode.Escape))
            {
                onClickPause();
            }
        }
    }
}
