using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outputs : MonoBehaviour
{
    [SerializeField] private GameObject levelLogic;
    [SerializeField] private GameObject field;
    [SerializeField] private GameObject play;

    public bool use(int[,] player, int position, int currentField)
    {
        int input = levelLogic.GetComponent<LevelLogic>().GetOutputs()[currentField][position].inputs[0];
        bool hasMoved;
        if (levelLogic.GetComponent<LevelLogic>().GetLogicGates(3)[currentField].Length > 0)
            hasMoved = GetComponent<GateOfLogic>().use(3, player, input, currentField);
        else if (levelLogic.GetComponent<LevelLogic>().GetLogicGates(2)[currentField].Length > 0)
            hasMoved = GetComponent<GateOfLogic>().use(2, player, input, currentField);
        else if (levelLogic.GetComponent<LevelLogic>().GetLogicGates(1)[currentField].Length > 0)
            hasMoved = GetComponent<GateOfLogic>().use(1, player, input, currentField);
        else
            hasMoved = GetComponent<Inputs>().use(player, input, currentField);

        levelLogic.GetComponent<LevelLogic>().colorOutput(position, hasMoved, hasMoved);
        if (hasMoved)
        {
            switch (levelLogic.GetComponent<LevelLogic>().GetOutputs()[currentField][position].type)
            {
                case "wait":
                    wait(player, position, currentField);
                    break;
                case "walk":
                    walk(player, position, currentField);
                    break;
                case "turn":
                    turn(player, position, currentField);
                    break;
                case "jump":
                    jump(player, position, currentField);
                    break;
                case "write":
                    write(player, position, currentField, true);
                    break;
                default:
                    break;
            }
        }
        else if (levelLogic.GetComponent<LevelLogic>().GetOutputs()[currentField][position].type.Equals("write"))
        {
            write(player, position, currentField, false);
        }
        return hasMoved;
    }

    private void wait(int[,] player, int position, int currentField)
    {

    }
    private void walk(int[,] player, int position, int currentField)
    {
        int x, y;
        switch (player[0, 2])
        {
              case 0:
                x = player[0, 0];
                y = player[0, 1] + 1;
                break;
            case 1:
                x = player[0, 0] + 1;
                y = player[0, 1];
                break;
            case 2:
                x = player[0, 0];
                y = player[0, 1] - 1;
                break;
            case 3:
                x = player[0, 0] - 1;
                y = player[0, 1];
                break;
            default:
                x = player[0, 0];
                y = player[0, 1];
                break;
        }
        string[] tile = field.GetComponent<LevelField>().getField(x, y); 
        if (isWin(tile))
        {
            field.GetComponent<LevelField>().setField(player[0, 0], player[0, 1], new Field(player[0, 0], player[0, 1], "empty", ""));
            play.GetComponent<Play>().wonPlayer();
        }
        else if (isKill(tile))
        {
            field.GetComponent<LevelField>().setField(player[0, 0], player[0, 1], new Field(player[0, 0], player[0, 1], "empty", ""));
        }
        else if (isWalkable(tile))
        {
            field.GetComponent<LevelField>().moveField(player[0, 0], player[0, 1], x, y);
            if (isTrigger(tile))
                trigger();
        }
    }
    private void turn(int[,] player, int position, int currentField)
    {
        string direction = levelLogic.GetComponent<LevelLogic>().GetOutputs()[currentField][position].specification;
        switch (direction)
        {
            case "left":
                player[0, 2] = player[0, 2] - 1;
                if (player[0, 2] < 0)
                    player[0, 2] = 3;
                break;
            case "right":
                player[0, 2] = player[0, 2] + 1;
                if (player[0, 2] > 3)
                    player[0, 2] = 0;
                break;
            default:
                Debug.Log("Error: Wrong direction");
                break;
        }
        Field turnedPlayer = new Field(player[0, 0], player[0, 1], "player", TypesOfObjects.getSpecificationsForType("player")[player[0, 2]]);
        field.GetComponent<LevelField>().setField(player[0, 0], player[0, 1], turnedPlayer);
    }
    private void jump(int[,] player, int position, int currentField)
    {
        int x, y, x2, y2;
        switch (player[0, 2])
        {
            case 0:
                x = player[0, 0];
                y = player[0, 1] + 1;
                x2 = player[0, 0];
                y2 = player[0, 1] + 2;
                break;
            case 1:
                x = player[0, 0] + 1;
                y = player[0, 1];
                x2 = player[0, 0] + 2;
                y2 = player[0, 1];
                break;
            case 2:
                x = player[0, 0];
                y = player[0, 1] - 1;
                x2 = player[0, 0];
                y2 = player[0, 1] - 2;
                break;
            case 3:
                x = player[0, 0] - 1;
                y = player[0, 1];
                x2 = player[0, 0] - 2;
                y2 = player[0, 1];
                break;
            default:
                x = player[0, 0];
                y = player[0, 1];
                x2 = player[0, 0];
                y2 = player[0, 1];
                break;
        }
        string[] nextTile = field.GetComponent<LevelField>().getField(x, y);
        string[] nextNextTile = field.GetComponent<LevelField>().getField(x2, y2);
        if (isWin(nextTile))
        {
            field.GetComponent<LevelField>().setField(player[0, 0], player[0, 1], new Field(player[0, 0], player[0, 1], "empty", ""));
            play.GetComponent<Play>().wonPlayer();
        }
        else if (isJumpable(nextTile))
        {
            if (isWin(nextNextTile))
            {
                field.GetComponent<LevelField>().setField(player[0, 0], player[0, 1], new Field(player[0, 0], player[0, 1], "empty", ""));
                play.GetComponent<Play>().wonPlayer();
            }
            else if (isKill(nextNextTile))
            {
                field.GetComponent<LevelField>().setField(player[0, 0], player[0, 1], new Field(player[0, 0], player[0, 1], "empty", ""));
            }
            else if (isWalkable(nextNextTile))
            {
                field.GetComponent<LevelField>().moveField(player[0, 0], player[0, 1], x2, y2);
                if (nextTile[1].Equals("fake"))
                {
                    field.GetComponent<LevelField>().setField(x, y, new Field(x, y, "empty", ""));
                }
                if (isTrigger(nextNextTile))
                    trigger();
            }
            else if (isKill(nextTile))
            {
                field.GetComponent<LevelField>().setField(player[0, 0], player[0, 1], new Field(player[0, 0], player[0, 1], "empty", ""));
            }
            else if (isWalkable(nextTile))
            {
                field.GetComponent<LevelField>().moveField(player[0, 0], player[0, 1], x, y);
                if (isTrigger(nextTile))
                    trigger();
            }
        }
    }
    private void write(int[,] player, int position, int currentField, bool value)
    {
        string where = levelLogic.GetComponent<LevelLogic>().GetOutputs()[currentField][position].specification;
        switch (where)
        {
            case "A":
                gameObject.GetComponent<Inputs>().write(value, 0);
                break;
            case "B":
                gameObject.GetComponent<Inputs>().write(value, 1);
                break;
            case "C":
                gameObject.GetComponent<Inputs>().write(value, 2);
                break;
            case "D":
                gameObject.GetComponent<Inputs>().write(value, 3);
                break;
            default:
                Debug.Log("Error: There is no memory here.");
                break;
        }
    }

    private bool isWalkable(string[] tile)
    {
        if (tile[0].Equals("empty") || tile[0].Equals("switch") || tile[1].Equals("fake"))
            return true;
        return false;
    }
    private bool isJumpable(string[] tile)
    {
        if (tile[0].Equals("empty") || tile[0].Equals("hole") || tile[0].Equals("switch") || tile[1].Equals("fake"))
            return true;
        return false;
    }
    private bool isWin(string[] tile)
    {
        if (tile[0].Equals("goal"))
            return true;
        return false;
    }
    private bool isKill(string[] tile)
    {
        if (tile[0].Equals("hole"))
            return true;
        return false;
    }
    private bool isTrigger(string[] tile)
    {
        if (tile[0].Equals("switch"))
            return true;
        return false;
    }

    private void trigger()
    {
        int[] dim = field.GetComponent<LevelField>().getDimensions();
        for (int i = 0; i < dim[0]; i++)
        {
            for (int j = 0; j < dim[1]; j++)
            {
                string[] tile = field.GetComponent<LevelField>().getField(i, j);
                if (tile[1].Equals("toEmpty"))
                {
                    field.GetComponent<LevelField>().setField(i, j, new Field(i, j, "empty", ""));
                }
                else if (tile[1].Equals("toWall"))
                {
                    field.GetComponent<LevelField>().setField(i, j, new Field(i, j, "wall", ""));
                }
            }
        }
    }
}
