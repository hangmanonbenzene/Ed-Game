using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TypeButton : MonoBehaviour
{
    string type;
    bool selected;

    public void setType(string type, string symbol)
    {
        this.type = type;
        GetComponentInChildren<TextMeshProUGUI>().text = symbol;
    }
    public string getType()
    {
          return type;
    }
    public void select(bool select)
    {
        if (select)
        {
            GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
            GetComponentInChildren<TextMeshProUGUI>().color = new Color(1, 1, 1);
            selected = true;
        }
        else
        {
            GetComponent<Image>().color = new Color(1, 1, 1);
            GetComponentInChildren<TextMeshProUGUI>().color = new Color32(50, 50, 50, 255);
            selected = false;
        }
    }
    public bool isSelected()
    {
        return selected;
    }
}
