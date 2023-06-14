using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelLogic : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject buttonHolder;
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private GameObject lineHolder;
    [SerializeField] private List<GameObject> lines;
    [SerializeField] private GameObject chooseButton;
    [SerializeField] private GameObject chooseHolder;
    [SerializeField] private GameObject[] chooseButtons;

    [SerializeField] private GameObject prompt;

    private int currentField;
    private int[] selectedButton;

    private SensorInput[][] inputs;
    private LogicGate[][] rowOne;
    private LogicGate[][] rowTwo;
    private LogicGate[][] rowThree;
    private SensorOutput[][] outputs;

    private int maxXCoordinate = 300;
    private int minXCoordinate = -300;
    private int maxYCoordinate = 340;
    private int minYCoordinate = -280;

    public void setupLogic(LogicField[] logicFields)
    {
        int numberOfFields = logicFields.Length;

        chooseButtons = new GameObject[numberOfFields];

        inputs = new SensorInput[numberOfFields][];
        rowOne = new LogicGate[numberOfFields][];
        rowTwo = new LogicGate[numberOfFields][];
        rowThree = new LogicGate[numberOfFields][];
        outputs = new SensorOutput[numberOfFields][];

        for (int i = 0; i < numberOfFields; i++)
        {
            int numberOfInputs = logicFields[i].sensorInputs.Length;
            inputs[i] = new SensorInput[numberOfInputs];
            for (int j = 0; j < numberOfInputs; j++)
            {
                inputs[i][j] = logicFields[i].sensorInputs[j];
            }

            int[] numberOfLogic = new int[3];
            foreach (LogicGate logicGate in logicFields[i].logicGates)
            {
                numberOfLogic[logicGate.row]++;
            }
            rowOne[i] = new LogicGate[numberOfLogic[0]];
            rowTwo[i] = new LogicGate[numberOfLogic[1]];
            rowThree[i] = new LogicGate[numberOfLogic[2]];
            foreach (LogicGate logicGate in logicFields[i].logicGates)
            {
                switch (logicGate.row)
                {
                    case 0:
                        rowOne[i][logicGate.position] = logicGate;
                        break;
                    case 1:
                        rowTwo[i][logicGate.position] = logicGate;
                        break;
                    case 2:
                        rowThree[i][logicGate.position] = logicGate;
                        break;
                }
            }

            int numberOfOutputs = logicFields[i].sensorOutputs.Length;
            outputs[i] = new SensorOutput[numberOfOutputs];
            for (int j = 0; j < numberOfOutputs; j++)
            {
                outputs[i][j] = logicFields[i].sensorOutputs[j];
            }

            GameObject button = Instantiate(chooseButton, chooseHolder.transform);
            int x = (numberOfFields - 1) * (-60) + 120 * i;
            int y = -370;
            button.transform.localPosition = new Vector2(x, y);
            button.GetComponentInChildren<TextMeshProUGUI>().text = (i + 1).ToString();
            int number = i;
            button.GetComponent<Button>().onClick.AddListener(() => onClickChoose(number));
            chooseButtons[i] = button;
        }
        repairWrongInputs();
        onClickChoose(0);
    }

    private void onClickChoose(int number)
    {
        currentField = number;
        selectChooseButton(number);
        setField(number);
    }
    private void onClickButton(int row, int number)
    {
        selectedButton = new int[] { row, number };
        int numberOfRows = 0;
        numberOfRows += (rowOne[currentField].Length > 0) ? 1 : 0;
        numberOfRows += (rowTwo[currentField].Length > 0) ? 1 : 0;
        numberOfRows += (rowThree[currentField].Length > 0) ? 1 : 0;

        if (row == 0)
        {
            prompt.GetComponent<LogicPrompt>().setInput(inputs[currentField][number]);
        }
        else if (row == numberOfRows + 1)
        {
            prompt.GetComponent<LogicPrompt>().setOutput(outputs[currentField][number]);
        }
        else if (row == 1 && row <= numberOfRows)
        {
            bool hasmultipleInputs = rowOne[currentField][number].inputs.Length > 1;
            prompt.GetComponent<LogicPrompt>().setLogic(rowOne[currentField][number], hasmultipleInputs);
        }
        else if (row == 2 && row <= numberOfRows)
        {
            bool hasmultipleInputs = rowTwo[currentField][number].inputs.Length > 1;
            prompt.GetComponent<LogicPrompt>().setLogic(rowTwo[currentField][number], hasmultipleInputs);
        }
        else if (row == 3 && row <= numberOfRows)
        {
            bool hasmultipleInputs = rowThree[currentField][number].inputs.Length > 1;
            prompt.GetComponent<LogicPrompt>().setLogic(rowThree[currentField][number], hasmultipleInputs);
        }
    }

    private void selectChooseButton(int number)
    {
        for (int i = 0; i < chooseButtons.Length; i++)
        {
            if (i == number)
            {
                chooseButtons[i].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                chooseButtons[i].GetComponentInChildren<TextMeshProUGUI>().color = new Color(1, 1, 1);
            }
            else
            {
                chooseButtons[i].GetComponent<Image>().color = new Color(1, 1, 1);
                chooseButtons[i].GetComponentInChildren<TextMeshProUGUI>().color = new Color32(50, 50, 50, 255);
            }
        }
    }
    private void setField(int number)
    {
        clearField();

        int numberOfRows = 0;
        numberOfRows += (rowOne[number].Length > 0) ? 1 : 0;
        numberOfRows += (rowTwo[number].Length > 0) ? 1 : 0;
        numberOfRows += (rowThree[number].Length > 0) ? 1 : 0;

        int[] gateLengths = new int[numberOfRows + 1];
        gateLengths[0] = inputs[number].Length;
        for (int i = 1; i <= numberOfRows; i++)
        {
            switch (i)
            {
                case 1:
                    gateLengths[i] = rowOne[number].Length;
                    break;
                case 2:
                    gateLengths[i] = rowTwo[number].Length;
                    break;
                case 3:
                    gateLengths[i] = rowThree[number].Length;
                    break;
            }
        }

        int numberOfInputs = inputs[number].Length;
        int numberOfGates = rowOne[number].Length + rowTwo[number].Length + rowThree[number].Length;
        int numberOfOutputs = outputs[number].Length;

        int numberOfButtons = numberOfInputs + numberOfGates + numberOfOutputs;
        buttons = new GameObject[numberOfButtons];
        int numberForButtons = 0;
        for (int i = 0; i < numberOfRows + 2; i++)
        {
            int numberOfButtonsInThisRow = 0;
            if (i == 0)
                numberOfButtonsInThisRow = numberOfInputs;
            else if (i == numberOfRows + 1)
                numberOfButtonsInThisRow = numberOfOutputs;
            else if (i == 1 && i <= numberOfRows)
                numberOfButtonsInThisRow = rowOne[number].Length;
            else if (i == 2 && i <= numberOfRows)
                numberOfButtonsInThisRow = rowTwo[number].Length;
            else if (i == 3 && i <= numberOfRows)
                numberOfButtonsInThisRow = rowThree[number].Length;
            for (int j = 0; j < numberOfButtonsInThisRow; j++)
            {
                int row = i;
                int numberInRow = j;
                StartCoroutine(setButtonSymbol(row, numberInRow));
                GameObject button = Instantiate(buttonPrefab, buttonHolder.transform);
                button.GetComponent<Button>().onClick.AddListener(() => onClickButton(row, numberInRow));
                bool onlyOneButton = (numberOfButtonsInThisRow == 1);
                int x = onlyOneButton ? 0 : minXCoordinate + (maxXCoordinate - minXCoordinate) / (numberOfButtonsInThisRow - 1) * numberInRow;
                int y = maxYCoordinate - (maxYCoordinate - minYCoordinate) / (numberOfRows + 1) * row;
                button.transform.localPosition = new Vector2(x, y);

                if (row != 0)
                {
                    int[] inputs = new int[0];
                    if (row == numberOfRows + 1)
                        inputs = outputs[number][numberInRow].inputs;
                    else if (i == 1 && i <= numberOfRows)
                        inputs = rowOne[number][numberInRow].inputs;
                    else if (i == 2 && i <= numberOfRows)
                        inputs = rowTwo[number][numberInRow].inputs;
                    else if (i == 3 && i <= numberOfRows)
                        inputs = rowThree[number][numberInRow].inputs;
                    if (inputs.Length > 2 || (row == numberOfRows + 1 && inputs.Length > 1))
                    {
                        Debug.Log("Error: too many inputs in row " + row);
                    }
                    else if (inputs.Length == 0)
                    {
                        Debug.Log("Error: no inputs in row " + row);
                    }
                    foreach (int input in inputs)
                    {
                        int lengthOfRowAbove = gateLengths[row - 1];
                        if (input < lengthOfRowAbove)
                        {
                            GameObject line = Instantiate(linePrefab, lineHolder.transform);
                            line.GetComponent<LineRenderer>().SetPosition(0, button.transform.position + new Vector3(0, 0.3f, 0));
                            int numberOfButton = numberForButtons - (numberInRow + 1) + (input - lengthOfRowAbove + 1);
                            Vector2 position = buttons[numberOfButton].transform.position + new Vector3(0, -0.3f, 0);
                            line.GetComponent<LineRenderer>().SetPosition(1, position);
                            lines.Add(line);
                        }
                        else
                        {
                            Debug.Log("Error: input has no target in row " + row);
                        }
                    }
                }

                buttons[numberForButtons] = button;
                numberForButtons++;
            }
        }
    }
    private void clearField()
    {
        foreach (GameObject button in buttons)
        {
            Destroy(button);
        }
        foreach (GameObject line in lines)
        {
            Destroy(line);
        }
    }
    private IEnumerator setButtonSymbol(int row, int number)
    {
        yield return null;
        int[] rowLengths = new int[4];
        rowLengths[0] = inputs[currentField].Length;
        rowLengths[1] = rowOne[currentField].Length;
        rowLengths[2] = rowTwo[currentField].Length;
        rowLengths[3] = rowThree[currentField].Length;
        int numberOfRows = 0;
        numberOfRows += (rowOne[currentField].Length > 0) ? 1 : 0;
        numberOfRows += (rowTwo[currentField].Length > 0) ? 1 : 0;
        numberOfRows += (rowThree[currentField].Length > 0) ? 1 : 0;

        if (row == 0)
        {
            string type = inputs[currentField][number].type;
            string specOne = inputs[currentField][number].specificationOne;
            string specTwo = inputs[currentField][number].specificationTwo;
            string specThree = inputs[currentField][number].specificationThree;
            buttons[number].GetComponentInChildren<TextMeshProUGUI>().text = TypesOfInputs.getSymbolForType(type, specOne, specTwo, specThree);
        }
        else if (row == numberOfRows + 1)
        {
            int buttonNumber = number + rowLengths[0] + rowLengths[1] + rowLengths[2] + rowLengths[3];
            string type = outputs[currentField][number].type;
            string spec = outputs[currentField][number].specification;
            buttons[buttonNumber].GetComponentInChildren<TextMeshProUGUI>().text = TypesOfOutputs.getSymbolForType(type, spec);
        }
        else if (row == 1 && row <= numberOfRows)
        {
            int buttonNumber = number + rowLengths[0];
            string type = rowOne[currentField][number].type;
            buttons[buttonNumber].GetComponentInChildren<TextMeshProUGUI>().text = TypesOfLogic.getSymbolForType(type);
        }
        else if (row == 2 && row <= numberOfRows)
        {
            int buttonNumber = number + rowLengths[0] + rowLengths[1];
            string type = rowTwo[currentField][number].type;
            buttons[buttonNumber].GetComponentInChildren<TextMeshProUGUI>().text = TypesOfLogic.getSymbolForType(type);
        }
        else if (row == 3 && row <= numberOfRows)
        {
            int buttonNumber = number + rowLengths[0] + rowLengths[1] + rowLengths[2];
            string type = rowThree[currentField][number].type;
            buttons[buttonNumber].GetComponentInChildren<TextMeshProUGUI>().text = TypesOfLogic.getSymbolForType(type);
        }
    }
    private void repairWrongInputs()
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            int numberOfRows = 0;
            numberOfRows += (rowOne[i].Length > 0) ? 1 : 0;
            numberOfRows += (rowTwo[i].Length > 0) ? 1 : 0;
            numberOfRows += (rowThree[i].Length > 0) ? 1 : 0;

            int[] gateLengths = new int[numberOfRows + 1];
            gateLengths[0] = inputs[i].Length;
            for (int j = 1; j <= numberOfRows; j++)
            {
                switch (j)
                {
                    case 1:
                        gateLengths[j] = rowOne[i].Length;
                        break;
                    case 2:
                        gateLengths[j] = rowTwo[i].Length;
                        break;
                    case 3:
                        gateLengths[j] = rowThree[i].Length;
                        break;
                }
            }
            
            for (int row = 0; row <= numberOfRows; row++)
            {
                int numberOfButtonsInThisRow = 0;
                if (row == numberOfRows)
                    numberOfButtonsInThisRow = outputs[i].Length;
                else if (row == 0 && row < numberOfRows)
                    numberOfButtonsInThisRow = rowOne[i].Length;
                else if (row == 1 && row < numberOfRows)
                    numberOfButtonsInThisRow = rowTwo[i].Length;
                else if (row == 2 && row < numberOfRows)
                    numberOfButtonsInThisRow = rowThree[i].Length;
                for (int position = 0; position < numberOfButtonsInThisRow; position++)
                {
                    if (row == numberOfRows)
                    {
                        if (outputs[i][position].inputs.Length == 0)
                            outputs[i][position].inputs = new int[] { 0 };
                        else if (outputs[i][position].inputs.Length > 1)
                            outputs[i][position].inputs = new int[] { outputs[i][position].inputs[0] };
                        if (outputs[i][position].inputs[0] >= gateLengths[row])
                            outputs[i][position].inputs[0] = gateLengths[row] - 1;
                    }
                    else if (row == 0 && row < numberOfRows)
                    {
                        if (rowOne[i][position].inputs.Length == 0)
                            rowOne[i][position].inputs = new int[] { 0 };
                        else if (rowOne[i][position].inputs.Length > 2)
                        {
                            int[] inputs = rowOne[i][position].inputs;
                            rowOne[i][position].inputs = new int[2];
                            for (int j = 0; j < 2; j++)
                                rowOne[i][position].inputs[j] = inputs[j];
                        }
                        if (rowOne[i][position].inputs.Length == 2)
                        {
                            if (rowOne[i][position].inputs[0] >= gateLengths[row])
                            {
                                int[] inputs = rowOne[i][position].inputs;
                                rowOne[i][position].inputs = new int[] { inputs[1] };
                            }
                            else if (rowOne[i][position].inputs[1] >= gateLengths[row])
                            {
                                int[] inputs = rowOne[i][position].inputs;
                                rowOne[i][position].inputs = new int[] { inputs[0] };
                            }
                        }
                        if (rowOne[i][position].inputs.Length == 1 && rowOne[i][position].inputs[0] >= gateLengths[row])
                            rowOne[i][position].inputs = new int[] { 0 };
                    }
                    else if (row == 1 && row < numberOfRows)
                    {
                        if (rowTwo[i][position].inputs.Length == 0)
                            rowTwo[i][position].inputs = new int[] { 0 };
                        else if (rowTwo[i][position].inputs.Length > 2)
                        {
                            int[] inputs = rowTwo[i][position].inputs;
                            rowTwo[i][position].inputs = new int[2];
                            for (int j = 0; j < 2; j++)
                                rowTwo[i][position].inputs[j] = inputs[j];
                        }
                        if (rowTwo[i][position].inputs.Length == 2)
                        {
                            if (rowTwo[i][position].inputs[0] >= gateLengths[row])
                            {
                                int[] inputs = rowTwo[i][position].inputs;
                                rowTwo[i][position].inputs = new int[] { inputs[1] };
                            }
                            else if (rowTwo[i][position].inputs[1] >= gateLengths[row])
                            {
                                int[] inputs = rowTwo[i][position].inputs;
                                rowTwo[i][position].inputs = new int[] { inputs[0] };
                            }
                        }
                        if (rowTwo[i][position].inputs.Length == 1 && rowTwo[i][position].inputs[0] >= gateLengths[row])
                            rowTwo[i][position].inputs = new int[] { 0 };
                    }
                    else if (row == 2 && row < numberOfRows)
                    {
                        if (rowThree[i][position].inputs.Length == 0)
                            rowThree[i][position].inputs = new int[] { 0 };
                        else if (rowThree[i][position].inputs.Length > 2)
                        {
                            int[] inputs = rowThree[i][position].inputs;
                            rowThree[i][position].inputs = new int[2];
                            for (int j = 0; j < 2; j++)
                                rowThree[i][position].inputs[j] = inputs[j];
                        }
                        if (rowThree[i][position].inputs.Length == 2)
                        {
                            if (rowThree[i][position].inputs[0] >= gateLengths[row])
                            {
                                int[] inputs = rowThree[i][position].inputs;
                                rowThree[i][position].inputs = new int[] { inputs[1] };
                            }
                            else if (rowThree[i][position].inputs[1] >= gateLengths[row])
                            {
                                int[] inputs = rowThree[i][position].inputs;
                                rowThree[i][position].inputs = new int[] { inputs[0] };
                            }
                        }
                        if (rowThree[i][position].inputs.Length == 1 && rowThree[i][position].inputs[0] >= gateLengths[row])
                            rowThree[i][position].inputs = new int[] { 0 };
                    }
                }
            }
        }
    }

    public void setLogic(LogicGate logicGate)
    {
        int numberOfRows = 0;
        numberOfRows += (rowOne[currentField].Length > 0) ? 1 : 0;
        numberOfRows += (rowTwo[currentField].Length > 0) ? 1 : 0;
        numberOfRows += (rowThree[currentField].Length > 0) ? 1 : 0;

        if (selectedButton[0] == 1 && selectedButton[0] <= numberOfRows)
        {
            rowOne[currentField][selectedButton[1]] = logicGate;
        }
        else if (selectedButton[0] == 2 && selectedButton[0] <= numberOfRows)
        {
            rowTwo[currentField][selectedButton[1]] = logicGate;
        }
        else if (selectedButton[0] == 3 && selectedButton[0] <= numberOfRows)
        {
            rowThree[currentField][selectedButton[1]] = logicGate;
        }
        StartCoroutine(setButtonSymbol(selectedButton[0], selectedButton[1]));
    }
    public SensorOutput[][] GetOutputs()
    {
        return outputs;
    }
    public LogicGate[][] GetLogicGates(int row)
    {
        if (row == 1)
            return rowOne;
        else if (row == 2)
            return rowTwo;
        else if (row == 3)
            return rowThree;
        else
            return null;
    }
    public SensorInput[][] GetInputs()
    {
        return inputs;
    }
}
