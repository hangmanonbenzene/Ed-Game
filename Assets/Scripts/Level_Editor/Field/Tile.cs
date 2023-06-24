using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    private string type;
    private int specification;
    private int xCoordinate;
    private int yCoordinate;

    private GameObject theField;

    protected static bool isPressed = false;
    private bool action = false;
    public void setup(string type, string specification)
    {
        this.type = type;
        this.specification = Array.IndexOf(TypesOfObjects.getSpecificationsForType(type), specification);

        GetComponentInChildren<TextMeshProUGUI>().text = TypesOfObjects.getSymbolForType(type, specification);
        setImageSize();
    }
    public string getType()
    {
        return type;
    }
    public string getSpecification()
    {
        return TypesOfObjects.getSpecificationsForType(type)[specification];
    }
    public void setImageSize()
    {
        GetComponent<RectTransform>().sizeDelta = this.GetComponent<RectTransform>().sizeDelta;
    }
    public void setValues(GameObject field, int x, int y)
    {
        xCoordinate = x;
        yCoordinate = y;
        theField = field;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        if (!action)
        {
            action = true;
            theField.GetComponent<TheField>().onClickTile(xCoordinate, yCoordinate);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        action = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isPressed && !action)
        {
            action = true;
            theField.GetComponent<TheField>().onClickTile(xCoordinate, yCoordinate);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        action = false;
    }
}
