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
        if (!field.GetComponent<LevelField>().getField(x, y)[0].Equals("player") &&
            !field.GetComponent<LevelField>().getField(x, y)[0].Equals("wall") &&
            !field.GetComponent<LevelField>().getField(x, y)[0].Equals("o.o.B.") &&
            !field.GetComponent<LevelField>().getField(x, y)[0].Equals("goal"))
            field.GetComponent<LevelField>().moveField(player[0, 0], player[0, 1], x, y);
        else if (field.GetComponent<LevelField>().getField(x, y)[0].Equals("goal"))
        {
            //field.GetComponent<LevelField>().setField(player[0, 0], player[0, 1], new Field(player[0, 0], player[0, 1], "empty", ""));
            field.GetComponent<LevelField>().moveField(player[0, 0], player[0, 1], x, y);
            play.GetComponent<Play>().wonPlayer();
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
}
