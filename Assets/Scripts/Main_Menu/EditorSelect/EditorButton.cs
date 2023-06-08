using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditorButton : MonoBehaviour
{
    [SerializeField] private GameObject deleteButton;
    [SerializeField] private GameObject levelNameText;
    private string levelName;
    public void setupButton(int position, string levelName, EditorSelect editorSelect)
    {
        this.levelName = levelName;
        GetComponent<Button>().onClick.AddListener(delegate { editorSelect.loadLevel(levelName); });
        deleteButton.GetComponent<Button>().onClick.AddListener(delegate { editorSelect.activateAreYouSure(levelName); });
        if (position == 0)
        {
            deleteButton.SetActive(false);
            GetComponent<Button>().Select();
        }
        setPosition(position);
        this.levelNameText.GetComponent<TextMeshProUGUI>().text = levelName;
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
