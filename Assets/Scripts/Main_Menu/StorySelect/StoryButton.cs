using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryButton : SelectButton
{
    [SerializeField] private GameObject deleteButton;
    [SerializeField] private GameObject levelNameText;

    public void setupButton(int position, string name, StoryMode storyMode)
    {
        setLevelName(name);
        GetComponent<Button>().onClick.AddListener(delegate { storyMode.loadGame(name); });
        deleteButton.GetComponent<Button>().onClick.AddListener(delegate { storyMode.activateAreYouSure(name); });
        setPosition(position);
        levelNameText.GetComponent<TextMeshProUGUI>().text = name;
    }
}
