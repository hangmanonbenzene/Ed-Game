using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseLevel : MonoBehaviour
{
    [SerializeField] private GameObject levelLogic;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject lines;
    [SerializeField] private GameObject levelEditorButton;
    [SerializeField] private GameObject continueButton;

    private bool pauseButtonActive = true;
    private bool pauseMenuActive = false;

    public void onClickPause()
    {
        setPauseButtonActive(false);
        pauseMenuActive = true;
        levelEditorButton.GetComponent<Button>().interactable = LevelName.getStoryMode() ? false : true;
        pauseMenu.SetActive(true);
        lines.SetActive(false);
        continueButton.GetComponent<Button>().Select();
    }

    public void onClickContinue()
    {
        setPauseButtonActive(true);
        pauseMenuActive = false;
        pauseMenu.SetActive(false);
        lines.SetActive(true);
        levelLogic.GetComponent<LevelLogic>().selectButton();
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
        pauseButtonActive = active;
    }
    public bool getPauseMenuActive()
    {
        return pauseMenuActive;
    }

    private void Update()
    {
        if (pauseButtonActive)
        {
            if (Input.GetKeyDown("joystick button 1"))
            {
                onClickPause();
            }
        }
    }
}
