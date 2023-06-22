using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;

public class Inputs : MonoBehaviour
{
    [SerializeField] private GameObject levelLogic;
    [SerializeField] private GameObject field;

    private bool[] memory = new bool[] { false, false, false, false };

    public bool use( int[,] player, int position, int currentField)
    {
        bool output;
        switch (levelLogic.GetComponent<LevelLogic>().GetInputs()[currentField][position].type)
        {
            case "false":
                output = false;
                break;
            case "true":
                output = true;
                break;
            case "look":
                output = look(player, position, currentField);
                break;
            case "read":
                output = read(player, position, currentField);
                break;
            default:
                output = false;
                break;
        }
        levelLogic.GetComponent<LevelLogic>().colorInput(position, output);
        return output;
    }

    private bool look(int[,] player, int position, int currentField)
    {
        string where = levelLogic.GetComponent<LevelLogic>().GetInputs()[currentField][position].specificationOne;
        string what = levelLogic.GetComponent<LevelLogic>().GetInputs()[currentField][position].specificationTwo;
        if (what.Equals("o.o.B"))
            what = "wall";
        int howFar = 0;
        switch (levelLogic.GetComponent<LevelLogic>().GetInputs()[currentField][position].specificationThree)
        {
            case "one":
                howFar = 1;
                break;
            case "two":
                howFar = 2;
                break;
            case "three":
                howFar = 3;
                break;
            case "four":
                howFar = 4;
                break;
        }
        int direction = player[0, 2];
        switch (where)
        {
            case "right":
                direction = (direction + 1) % 4;
                break;
            case "left":
                direction = (direction + 3) % 4;
                break;
            case "backwards":
                direction = (direction + 2) % 4;
                break;
            default:
                break;
        }
        int x = player[0, 0];
        int y = player[0, 1];
        for (int i = 0; i < howFar; i++)
        {
            switch (direction)
            {
                case 0:
                    y++;
                    break;
                case 1:
                    x++;
                    break;
                case 2:
                    y--;
                    break;
                case 3:
                    x--;
                    break;
                default:
                    break;
            }
            if (field.GetComponent<LevelField>().getField(x, y)[0] == what)
                return true;
            if (field.GetComponent<LevelField>().getField(x, y)[0] == "wall")
                return false;
        }
        return false;
    }
    private bool read(int[,] player, int position, int currentField)
    {
        string which = levelLogic.GetComponent<LevelLogic>().GetInputs()[currentField][position].specificationOne;
        switch (which)
        {
            case "A":
                return memory[0];
            case "B":
                return memory[1];
            case "C":
                return memory[2];
            case "D":
                return memory[3];
            default:
                return false;
        }
    }

    public void write(bool input, int where)
    {
        memory[where] = input;
    }
    public void resetMemory()
    {
        memory = new bool[] { false, false, false, false };
    }
}
