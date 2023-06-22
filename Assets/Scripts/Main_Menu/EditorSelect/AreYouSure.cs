using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AreYouSure : MonoBehaviour
{
    private string levelName;
    [SerializeField] private GameObject levelNameText;
    [SerializeField] private GameObject editorSelect;
    [SerializeField] private GameObject levelSelect;
    [SerializeField] private GameObject storySelect;
    [SerializeField] private GameObject yesButton;
    [SerializeField] private GameObject popUp;
    private bool story;
    public void setupAreYouSure(string levelName, bool story)
    {
        this.levelName = levelName;
        this.story = story;
        levelNameText.GetComponent<TextMeshProUGUI>().text = levelName;
        popUp.SetActive(true);
        yesButton.GetComponent<Button>().Select();
    }

    public void onClickYes()
    {
        if (story)
        {
            SaveSystem.deleteStory(levelName);
            storySelect.GetComponent<StoryMode>().removeButton(levelName);
            storySelect.GetComponent<StoryMode>().resetButtons();
            storySelect.GetComponent<StoryMode>().setupButtons();
        }
        else
        {
            SaveSystem.deleteLevel(levelName);
            levelSelect.GetComponent<LevelSelect>().removeButton(levelName);
            editorSelect.GetComponent<EditorSelect>().removeButton(levelName);
            editorSelect.GetComponent<EditorSelect>().resetButtons();
            editorSelect.GetComponent<EditorSelect>().setupButtons();
        }
        popUp.SetActive(false);
    }
    public void onClickNo()
    {
        if (story)
        {
            storySelect.GetComponent<StoryMode>().enableButtons();
            StartCoroutine(storySelect.GetComponent<StoryMode>().SelectFirstButtonDelayed());
        }
        else
        {
            editorSelect.GetComponent<EditorSelect>().enableButtons();
            StartCoroutine(editorSelect.GetComponent<EditorSelect>().SelectFirstButtonDelayed());
        }
        popUp.SetActive(false);
    }
}
