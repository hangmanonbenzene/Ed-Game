using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryMode : Buttons
{
    [SerializeField] private GameObject newGamebutton;
    [SerializeField] private GameObject areYouSure;
    [SerializeField] private GameObject newGamePrompt;
    [SerializeField] private GameObject inputField;
    [SerializeField] private GameObject newGameButton;

    private bool promptActive;

    private void Start()
    {
        string[] saveGames = SaveSystem.getAllStoryNames();
        GameObject button = Instantiate(newGamebutton, transform);
        button.GetComponent<NewStoryButton>().setupButton(this);
        addButton(button);
        for (int i = 0; i < saveGames.Length; i++)
        {
            button = Instantiate(Button, transform);
            button.GetComponent<StoryButton>().setupButton(i + 1, saveGames[i], this);
            addButton(button);
        }
    }

    public void newGame()
    {
        disableButtons();
        newGamePrompt.SetActive(true);
        promptActive = true;
        inputField.GetComponent<TMP_InputField>().Select();
    }
    public void loadGame(string saveName)
    {
        LevelName.setStoryMode(true);
        mainMenu.GetComponent<MainMenu>().goToScene(1, saveName);
    }
    public void activateAreYouSure(string levelName)
    {
        disableButtons();
        areYouSure.GetComponent<AreYouSure>().setupAreYouSure(levelName, true);
    }

    public void onClickCancel()
    {
        newGamePrompt.SetActive(false);
        inputField.GetComponent<TMP_InputField>().text = string.Empty;
        enableButtons();
        promptActive = false;
        StartCoroutine(SelectFirstButtonDelayed());
    }
    public void onClickNewGame()
    {
        string name = inputField.GetComponent<TMP_InputField>().text;
        if (!SaveSystem.existsStory(name) && !name.Equals(""))
            StartCoroutine(newGameCoroutine(name));
    }
    private IEnumerator newGameCoroutine(string name)
    {
        SaveSystem.saveStory(new StoryData(name, 0));
        yield return null;
        loadGame(name);
    }

    public void inputEnd()
    {
        newGameButton.GetComponent<Button>().Select();
    }

    private void Update()
    {
        base.Update();
        if (promptActive && Input.GetKeyDown("joystick button 0"))
            onClickNewGame();
        if (promptActive && Input.GetKeyDown("joystick button 1"))
            onClickCancel();
    }
}
