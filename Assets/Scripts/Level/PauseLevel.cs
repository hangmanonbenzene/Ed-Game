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
    [SerializeField] private GameObject play;

    private bool pauseButtonActive = true;
    private bool pauseMenuActive = false;

    public void onClickPause()
    {
        if (pauseMenuActive)
        {
            onClickContinue();
        }
        else
        {
            pauseMenuActive = true;
            levelEditorButton.GetComponent<Button>().interactable = LevelName.getStoryMode() ? false : true;
            pauseMenu.SetActive(true);
            lines.SetActive(false);
            levelLogic.GetComponent<LevelLogic>().setChooseButtonsActive(false);
            continueButton.GetComponent<Button>().Select();
            GetComponent<TimePerTick>().disableTimeOnScreen();
            if (play.GetComponent<Play>().getIsPlay() && !play.GetComponent<Play>().getIsPause())
            {
                play.GetComponent<Play>().onCLickPause();
            }
            play.GetComponent<Play>().setPlayButtonActive(false);
            play.GetComponent<Play>().setPauseButtonActive(false);
        }
    }

    public void onClickContinue()
    {
        pauseMenuActive = false;
        pauseMenu.SetActive(false);
        lines.SetActive(true);
        if (!play.GetComponent<Play>().getIsPlay())
        {
            levelLogic.GetComponent<LevelLogic>().selectButton();
            levelLogic.GetComponent<LevelLogic>().setChooseButtonsActive(true);
        }
        else
            play.GetComponent<Play>().setPauseButtonActive(true);
        play.GetComponent<Play>().setPlayButtonActive(true);
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
            if (Input.GetKeyDown("joystick button 7"))
            {
                onClickPause();
            }
        }
    }
}
