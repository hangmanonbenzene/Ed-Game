using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectButton : MonoBehaviour
{
    private string levelName;

    public void setLevelName(string levelName)
    {
        this.levelName = levelName;
    }
    public string getLevelName()
    {
        return levelName;
    }
    public void setPosition(int position)
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 300 - 200 * position);
    }
}
