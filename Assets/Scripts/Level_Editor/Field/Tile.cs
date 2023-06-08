using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private string type;
    private string specification;

    public void setup(string type, string specification)
    {
        this.type = type;
        this.specification = specification;

        GetComponentInChildren<TextMeshProUGUI>().text = TypesOfObjects.getSymbolForType(type, specification);
    }
    public string getType()
    {
        return type;
    }
    public string getSpecification()
    {
        return specification;
    }
}
