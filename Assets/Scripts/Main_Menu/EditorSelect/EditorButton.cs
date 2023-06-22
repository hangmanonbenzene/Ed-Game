using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditorButton : SelectButton
{
    [SerializeField] private GameObject deleteButton;
    [SerializeField] private GameObject levelNameText;
    public void setupButton(int position, string levelName, EditorSelect editorSelect)
    {
        setLevelName(levelName);
        GetComponent<Button>().onClick.AddListener(delegate { editorSelect.loadLevel(levelName); });
        deleteButton.GetComponent<Button>().onClick.AddListener(delegate { editorSelect.activateAreYouSure(levelName); });
        if (position == 0)
        {
            deleteButton.SetActive(false);
        }
        setPosition(position);
        this.levelNameText.GetComponent<TextMeshProUGUI>().text = levelName;
    }
}
