using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldMenu : MonoBehaviour
{
    [SerializeField] private GameObject components;

    public void setupField(Field[] field)
    {
        components.GetComponent<TheField>().setup(field);
        components.GetComponent<TheObjects>().setup();
    }

    public Field[] getField()
    {
        GameObject[,] tiles = components.GetComponent<TheField>().getField();
        int[] dimension = components.GetComponent<TheField>().getDimensions();
        Field[] field = new Field[dimension[0] * dimension[1]];

        int index = 0;
        for (int i = 0; i < dimension[0]; i++) 
        { 
            for (int j = 0; j < dimension[1]; j++)
            {
                field[index] = new Field(i, j, tiles[i, j].GetComponent<Tile>().getType(), tiles[i, j].GetComponent<Tile>().getSpecification());
                index++;
            }
        }
        return field;
    }
    public string[] getSelectedType()
    {
        string type = components.GetComponent<TheObjects>().getSelectedType();
        string specification = components.GetComponent<TheObjects>().getSelectedSpecification();

        return new string[] { type, specification };
    }
}
