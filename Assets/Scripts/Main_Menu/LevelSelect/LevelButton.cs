using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private GameObject levelNameText;
    private string levelName;

    public void setupButton(int position, string levelName, LevelSelect levelSelect)
    {
        this.levelName = levelName;
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { levelSelect.loadLevel(levelName); });
        setPosition(position);
        this.levelNameText.GetComponent<TMPro.TextMeshProUGUI>().text = levelName;
    }

    public void setPosition(int position)
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 300 - 200 * position);
    }
    public void move(int amount)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + 200 * amount);
    }
    public string getLevelName()
    {
        return levelName;
    }
}
