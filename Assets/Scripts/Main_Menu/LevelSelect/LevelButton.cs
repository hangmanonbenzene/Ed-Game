using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : SelectButton
{
    [SerializeField] private GameObject levelNameText;

    public void setupButton(int position, string levelName, LevelSelect levelSelect)
    {
        setLevelName(levelName);
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { levelSelect.loadLevel(levelName); });
        setPosition(position);
        this.levelNameText.GetComponent<TMPro.TextMeshProUGUI>().text = levelName;
    }
}
