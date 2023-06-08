using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainButtons : Buttons
{
    public void onClickLevelSelection()
    {
        disableButtons();
        mainMenu.GetComponent<MainMenu>().goToMenu(MainMenu.MenuState.LEVEL_SELECTION);
    }
    public void onClickEditorSelection()
    {
        disableButtons();
        mainMenu.GetComponent<MainMenu>().goToMenu(MainMenu.MenuState.EDITOR_SELECTION);
    }
    public void onClickOptions()
    {
        disableButtons();
        mainMenu.GetComponent<MainMenu>().goToMenu(MainMenu.MenuState.OPTIONS_MENU);
    }
    public void onClickQuit()
    {
        Application.Quit();
    }
}
