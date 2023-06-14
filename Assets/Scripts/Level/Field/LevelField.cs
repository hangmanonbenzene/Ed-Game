using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelField : MonoBehaviour
{
    [SerializeField] GameObject textures;

    private GameObject[,] field;
    private Field[,] objects;
    private int[] dimensions;
    private Field[] defaultField;

    public void setupField(Field[] field)
    {
        defaultField = field;
        int x = 0;
        int y = 0;
        foreach (Field f in field)
        {
            if (f.xCoordinate > x)
                x = f.xCoordinate;
            if (f.yCoordinate > y)
                y = f.yCoordinate;
        }
        this.field = new GameObject[x + 1, y + 1];
        this.objects = new Field[x + 1, y + 1];
        this.dimensions = new int[2] { x + 1, y + 1 };
        foreach (Field f in field)
        {
            string type = f.type;
            int spec = Array.IndexOf(TypesOfObjects.getSpecificationsForType(type), f.specification);
            GameObject texturePrefab = textures.GetComponent<FieldTextures>().getTextureForType(type, spec);
            this.field[f.xCoordinate, f.yCoordinate] = Instantiate(texturePrefab, this.transform);
            int xCoordinate = (f.xCoordinate - 3) * 144 + (6 - x) * 72;
            int yCoordinate = (f.yCoordinate - 3) * 144 + (6 - y) * 72;
            this.field[f.xCoordinate, f.yCoordinate].transform.localPosition = new Vector2(xCoordinate, yCoordinate);
            objects[f.xCoordinate, f.yCoordinate] = f;
        }
    }

    public void setField(int x, int y, Field field)
    {
        objects[x, y] = field;
        objects[x, y].xCoordinate = x;
        objects[x, y].yCoordinate = y;
        int xCoordinate = (int)this.field[x, y].transform.localPosition.x;
        int yCoordinate = (int)this.field[x, y].transform.localPosition.y;
        Destroy(this.field[x, y]);
        int spec = Array.IndexOf(TypesOfObjects.getSpecificationsForType(field.type), field.specification);
        GameObject texturePrefab = textures.GetComponent<FieldTextures>().getTextureForType(field.type, spec);
        this.field[x, y] = Instantiate(texturePrefab, this.transform);
        this.field[x, y].transform.localPosition = new Vector2(xCoordinate, yCoordinate);
    }
    public void moveField(int x, int y, int x1, int y1)
    {
        if (!(x1 < 0 || x1 >= dimensions[0] || y1 < 0 || y1 >= dimensions[1]))
            setField(x1, y1, objects[x, y]);
        setField(x, y, new Field(x, y, "empty", ""));
    }
    public void resetField()
    {
        for (int i = 0; i < dimensions[0]; i++)
        {
            for (int j = 0; j < dimensions[1]; j++)
            {
                setField(i, j, defaultField[i * dimensions[1] + j]);
            }
        }
    }
    public int[,] getPlayerPositions()
    {
        int count = 0;
        foreach (Field field in objects)
        {
            if (field.type == "player")
                count++;
        }
        int[,] positions = new int[count, 3];
        int i = 0;
        foreach (Field field in objects)
        {
            if (field.type == "player")
            {
                positions[i, 0] = field.xCoordinate;
                positions[i, 1] = field.yCoordinate;
                positions[i, 2] = Array.IndexOf(TypesOfObjects.getSpecificationsForType(field.type), field.specification);
                i++;
            }
        }
        return positions;
    }
    public string[] getField(int x, int y)
    {
        if (x < 0 || x >= dimensions[0] || y < 0 || y >= dimensions[1])
            return new string[2] { "empty", "" };
        return new string[2] { objects[x, y].type, objects[x, y].specification };
    }
    public int[] getDimensions()
    {
        return dimensions;
    }
}
