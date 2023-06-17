using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateOfLogic : MonoBehaviour
{
    [SerializeField] private GameObject levelLogic;
    [SerializeField] private GameObject play;
    public bool use(int row, int[,] player, int position, int currentField)
    {
        int[] inputs = levelLogic.GetComponent<LevelLogic>().GetLogicGates(row)[currentField][position].inputs;
        bool[] values = new bool[2];
        values[0] = row == 1 ? GetComponent<Inputs>().use(player, inputs[0], currentField) : use(row - 1, player, inputs[0], currentField);
        if (inputs.Length > 1)
            values[1] = row == 1 ? GetComponent<Inputs>().use(player,inputs[1], currentField) : use(row - 1, player, inputs[1], currentField);
        else
            values = new bool[] { values[0] };

        if (levelLogic.GetComponent<LevelLogic>().GetLogicGates(row)[currentField][position].type == "empty")
        {
            if (play.GetComponent<Play>().getIsPlay())
                play.GetComponent<Play>().onClickPlay();
            return false;
        }

        bool output = levelLogic.GetComponent<LevelLogic>().GetLogicGates(row)[currentField][position].type switch
        {
            "buffer" => values[0],
            "and" => values[0] && values[1],
            "or" => values[0] || values[1],
            "xor" => values[0] ^ values[1],
            "not" => !values[0],
            "nand" => !(values[0] && values[1]),
            "nor" => !(values[0] || values[1]),
            "xnor" => !(values[0] ^ values[1]),
            _ => false,
        };
        levelLogic.GetComponent<LevelLogic>().colorLogic(row, position, output, values);
        return output;
    }
}
