using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TheField : MonoBehaviour
{
    [SerializeField] private GameObject fieldMenu;

    [SerializeField] private GameObject field;
    [SerializeField] private GameObject width;
    [SerializeField] private GameObject height;

    [SerializeField] private GameObject tilePrefab;

    private GameObject[,] tiles;

    public void setup(Field[] field)
    {
        instantiateTheTiles();
        int rows = 0;
        int columns = 0;
        foreach (Field f in field)
        {
            if (f.xCoordinate > rows)
            {
                rows = f.xCoordinate;
            }
            if (f.yCoordinate > columns)
            {
                columns = f.yCoordinate;
            }
        }
        setupField(rows, columns);
        setTiles(field, rows, columns);

        width.GetComponent<Slider>().value = rows;
        height.GetComponent<Slider>().value = columns;
        width.GetComponent<Slider>().onValueChanged.AddListener(delegate { onChangeValue(); });
        height.GetComponent<Slider>().onValueChanged.AddListener(delegate { onChangeValue(); });
    }

    private void instantiateTheTiles()
    {
        tiles = new GameObject[7, 7];
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                tiles[i, j] = Instantiate(tilePrefab, this.field.transform);
                int x = i;
                int y = j;
                tiles[i, j].GetComponent<Button>().onClick.AddListener(() => onClickTile(x, y));
                tiles[i, j].GetComponent<RectTransform>().anchoredPosition = new Vector2(110 * i, 110 * j);
            }
        }
    }
    private void setupField(int rows, int columns)
    {
        // Activate the tiles that are part of the field and deactivate the others
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                if (i <= rows && j <= columns)
                {
                    tiles[i, j].SetActive(true);
                }
                else
                {
                    tiles[i, j].SetActive(false);
                }
            }
        }

        // Move the field so that the active tiles are centered
        Vector2 center = new Vector2(0, 0);
        center.x = -110 * rows / 2;
        center.y = -110 * columns / 2;
        this.field.GetComponent<RectTransform>().anchoredPosition = center;
    }
    private void setTiles(Field[] field, int rows, int columns)
    {
        foreach (GameObject tile in tiles)
        {
            tile.GetComponent<Tile>().setup("empty", "");
        }
        foreach (Field f in field)
        {
            tiles[f.xCoordinate, f.yCoordinate].GetComponent<Tile>().setup(f.type, f.specification);
        }
    }

    private void onClickTile(int x, int y)
    {
        string[] type = fieldMenu.GetComponent<FieldMenu>().getSelectedType();
        tiles[x, y].GetComponent<Tile>().setup(type[0], type[1]);
    }
    private void onChangeValue()
    {
        int rows = (int)width.GetComponent<Slider>().value;
        int columns = (int)height.GetComponent<Slider>().value;
        setupField(rows, columns);
    }

    public GameObject[,] getField()
    {
        return tiles;
    }
    public int[] getDimensions()
    {
        int[] dimensions = new int[2];
        dimensions[0] = (int)width.GetComponent<Slider>().value + 1;
        dimensions[1] = (int)height.GetComponent<Slider>().value + 1;
        return dimensions;
    }
}
