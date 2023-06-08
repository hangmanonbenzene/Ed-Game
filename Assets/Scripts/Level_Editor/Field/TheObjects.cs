using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TheObjects : MonoBehaviour
{
    private GameObject[,] objects;
    [SerializeField] private GameObject objectField;
    [SerializeField] private GameObject objectText;
    [SerializeField] private GameObject specification;
    [SerializeField] private GameObject specificationText;

    private string currentSelectedType;
    private int currentSelectedSpecification;

    [SerializeField] private GameObject objectPrefab;

    public void setup()
    {
        instantiateTheObjects();
        specification.GetComponent<Button>().onClick.AddListener(delegate { onClickSpecification(); });
    }

    private void instantiateTheObjects()
    {
        objects = new GameObject[3, 3];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                objects[i, j] = Instantiate(objectPrefab, objectField.transform);
                objects[i, j].GetComponent<RectTransform>().anchoredPosition = new Vector2(120 * (i - 1), 120 * (j - 1));
            }
        }
        // Activate as many buttons as there are objectTypes, deactivate the others
        int number = 0;
        string[] types = TypesOfObjects.getTypes();
        for (int j = 2; j >= 0; j--)
        {
            for (int i = 0; i < 3; i++)
            {
                if (number < types.Length)
                {
                    objects[i, j].SetActive(true);
                    setSymbolButton(objects[i, j], types[number]);
                    if (number == 0)
                        onClickObject(objects[i, j], types[number]);
                    number++;
                }
                else
                {
                    objects[i, j].SetActive(false);
                }
            }
        }
    }
    private void setSymbolButton(GameObject button, string type)
    {
        button.GetComponentInChildren<TextMeshProUGUI>().text = TypesOfObjects.getSymbolForType(type, "letter");
        button.GetComponent<Button>().onClick.AddListener(delegate { onClickObject(button, type); });
    }
    private void onClickObject(GameObject button, string type)
    {
        currentSelectedType = type;
        objectText.GetComponent<TextMeshProUGUI>().text = type;
        // Change the color of the button to black and the font color to white
        button.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
        button.GetComponentInChildren<TextMeshProUGUI>().color = new Color(1, 1, 1);
        foreach (GameObject obj in objects)
        {
            if (obj != button)
            {
                obj.GetComponent<Image>().color = new Color(1, 1, 1);
                obj.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(50, 50, 50, 255);
            }
        }
        // Change the symbol of the specification button and make the button uniteractable if there is only one specification
        string[] specifications = TypesOfObjects.getSpecificationsForType(type);
        if (specifications.Length == 1)
        {
            specification.GetComponent<Button>().interactable = false;
        }
        else
        {
            specification.GetComponent<Button>().interactable = true;
        }
        currentSelectedSpecification = 0;
        specificationText.GetComponent<TextMeshProUGUI>().text = TypesOfObjects.getSymbolForType(type, specifications[0]);
    }
    private void onClickSpecification()
    {
        string[] specifications = TypesOfObjects.getSpecificationsForType(currentSelectedType);
        int number = specifications.Length;

        currentSelectedSpecification++;
        if (currentSelectedSpecification >= number)
        {
            currentSelectedSpecification = 0;
        }
        specificationText.GetComponent<TextMeshProUGUI>().text = TypesOfObjects.getSymbolForType(currentSelectedType, specifications[currentSelectedSpecification]);
    }

    public string getSelectedType()
    {
        return currentSelectedType;
    }
    public string getSelectedSpecification()
    {
        return TypesOfObjects.getSpecificationsForType(currentSelectedType)[currentSelectedSpecification];
    }
}
