using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuButtons;
    [SerializeField] private GameObject levelSelectionButtons;
    [SerializeField] private GameObject editorSelectionButtons;
    [SerializeField] private GameObject optionsMenuButtons;

    private GameObject levelName;

    public enum MenuState
    {
        MAIN_MENU,
        LEVEL_SELECTION,
        EDITOR_SELECTION,
        OPTIONS_MENU
    }

    void Start()
    {
        levelName = GameObject.FindGameObjectWithTag("LevelName");
        StartCoroutine(startInMenu());
    }

    public void goToMenu(MenuState menuState)
    {
        switch (menuState)
        {
            case MenuState.MAIN_MENU:
                mainMenuButtons.GetComponent<MainButtons>().setupButtons();
                break;
            case MenuState.LEVEL_SELECTION:
                levelSelectionButtons.GetComponent<LevelSelect>().setupButtons();
                break;
            case MenuState.EDITOR_SELECTION:
                editorSelectionButtons.GetComponent<EditorSelect>().setupButtons();
                break;
            case MenuState.OPTIONS_MENU:
                optionsMenuButtons.GetComponent<OptionButtons>().setupButtons();
                break;
        }
    }
    public void goToScene(int sceneID, string levelName)
    {
        this.levelName.GetComponent<LevelName>().setLevelName(levelName);
        SceneManager.LoadScene(sceneID);
    }

    private IEnumerator startInMenu()
    {
        yield return null;
        switch (levelName.GetComponent<LevelName>().getMenu())
        {
            case "Level":
                goToMenu(MenuState.LEVEL_SELECTION);
                break;
            case "Editor":
                goToMenu(MenuState.EDITOR_SELECTION);
                break;
            default:
                goToMenu(MenuState.MAIN_MENU);
                break;
        }
    }
}
