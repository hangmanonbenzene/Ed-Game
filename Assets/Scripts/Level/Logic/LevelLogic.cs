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

    private int currentField = -1;
    private int[] selectedButton;

    private SensorInput[][] inputs;
    private LogicGate[][] rowOne;
    private LogicGate[][] rowTwo;
    private LogicGate[][] rowThree;
    private SensorOutput[][] outputs;

    private bool[][][] permanentGates;
    private int[] restrictedGates;

    int[] clickedButton = new int[] { -1, -1 };
    [SerializeField] private GameObject play;
    [SerializeField] private GameObject pause;
    private bool chooseButtonActive;

    private readonly int maxXCoordinate = 300;
    private readonly int minXCoordinate = -300;
    private readonly int maxYCoordinate = 340;
    private readonly int minYCoordinate = -280;
    
    public void setupLogic(LogicField[] logicFields, int[] restrictedGates)
    {
        int numberOfFields = logicFields.Length;
        this.restrictedGates = restrictedGates;

        chooseButtons = new GameObject[numberOfFields];

        inputs = new SensorInput[numberOfFields][];
        rowOne = new LogicGate[numberOfFields][];
        rowTwo = new LogicGate[numberOfFields][];
        rowThree = new LogicGate[numberOfFields][];
        outputs = new SensorOutput[numberOfFields][];
        permanentGates = new bool[numberOfFields][][];

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

            int numberOfRows = 0;
            numberOfRows += (rowOne[i].Length > 0) ? 1 : 0;
            numberOfRows += (rowTwo[i].Length > 0) ? 1 : 0;
            numberOfRows += (rowThree[i].Length > 0) ? 1 : 0;
            permanentGates[i] = new bool[numberOfRows][];
            for (int j = 0; j < numberOfRows; j++)
            {
                permanentGates[i][j] = new bool[numberOfLogic[j]];
                switch (j)
                {
                    case 0:
                        for (int k = 0; k < numberOfLogic[j]; k++)
                            permanentGates[i][j][k] = rowOne[i][k].type != "empty";
                        break;
                    case 1:
                        for (int k = 0; k < numberOfLogic[j]; k++)
                            permanentGates[i][j][k] = rowTwo[i][k].type != "empty";
                        break;
                    case 2:
                        for (int k = 0; k < numberOfLogic[j]; k++)
                            permanentGates[i][j][k] = rowThree[i][k].type != "empty";
                        break;
                }
            }

            GameObject button = Instantiate(chooseButton, chooseHolder.transform);
            int x = (numberOfFields - 1) * (-75) + 150 * i;
            int y = -370;
            button.transform.localPosition = new Vector2(x, y);
            button.GetComponentInChildren<TextMeshProUGUI>().text = (i + 1).ToString();
            int number = i;
            button.GetComponent<Button>().onClick.AddListener(() => onClickChoose(number));
            chooseButtons[i] = button;
        }
        repairWrongInputs();
        dealWithPermanentGates();
        onClickChoose(0);
    }

    public void onClickChoose(int number)
    {
        if (number == currentField)
            return;
        currentField = number;
        selectChooseButton(number);
        setField(number);
        clickedButton = new int[] { -1, -1 };
        if (!pause.GetComponent<PauseLevel>().getPauseMenuActive() && !play.GetComponent<Play>().getIsPlay())
            selectButton();
    }
    private void onClickButton(int row, int number)
    {
        clickedButton = new int[] { row, number };
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
            prompt.GetComponent<LogicPrompt>().setLogic(rowOne[currentField][number], hasmultipleInputs, restrictedGates);
        }
        else if (row == 2 && row <= numberOfRows)
        {
            bool hasmultipleInputs = rowTwo[currentField][number].inputs.Length > 1;
            prompt.GetComponent<LogicPrompt>().setLogic(rowTwo[currentField][number], hasmultipleInputs, restrictedGates);
        }
        else if (row == 3 && row <= numberOfRows)
        {
            bool hasmultipleInputs = rowThree[currentField][number].inputs.Length > 1;
            prompt.GetComponent<LogicPrompt>().setLogic(rowThree[currentField][number], hasmultipleInputs, restrictedGates);
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
                    {
                        inputs = rowOne[number][numberInRow].inputs;
                        if (permanentGates[number][0][numberInRow])
                            button.GetComponent<Button>().interactable = false;
                        else
                            button.GetComponent<Button>().interactable = true;
                    }
                    else if (i == 2 && i <= numberOfRows)
                    {
                        inputs = rowTwo[number][numberInRow].inputs;
                        if (permanentGates[number][1][numberInRow])
                            button.GetComponent<Button>().interactable = false;
                        else
                            button.GetComponent<Button>().interactable = true;
                    }
                    else if (i == 3 && i <= numberOfRows)
                    {
                        inputs = rowThree[number][numberInRow].inputs;
                        if (permanentGates[number][2][numberInRow])
                            button.GetComponent<Button>().interactable = false;
                        else
                            button.GetComponent<Button>().interactable = true;
                    }
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
        lines = new List<GameObject>();
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
                            outputs[i][position].inputs = new int[] { position };
                        else if (outputs[i][position].inputs.Length > 1)
                            outputs[i][position].inputs = new int[] { outputs[i][position].inputs[0] };
                        if (outputs[i][position].inputs[0] >= gateLengths[row])
                            outputs[i][position].inputs[0] = gateLengths[row] - 1;
                    }
                    else if (row == 0 && row < numberOfRows)
                    {
                        if (rowOne[i][position].inputs.Length == 0)
                            rowOne[i][position].inputs = new int[] { position };
                        else if (rowOne[i][position].inputs.Length > 2)
                        {
                            int[] inputs = rowOne[i][position].inputs;
                            rowOne[i][position].inputs = new int[] { inputs[0], inputs[1] };
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
                            rowOne[i][position].inputs[0] = gateLengths[row] - 1;
                    }
                    else if (row == 1 && row < numberOfRows)
                    {
                        if (rowTwo[i][position].inputs.Length == 0)
                            rowTwo[i][position].inputs = new int[] { position };
                        else if (rowTwo[i][position].inputs.Length > 2)
                        {
                            int[] inputs = rowTwo[i][position].inputs;
                            rowTwo[i][position].inputs = new int[] { inputs[0], inputs[1] };
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
                            rowTwo[i][position].inputs[0] = gateLengths[row] - 1;
                    }
                    else if (row == 2 && row < numberOfRows)
                    {
                        if (rowThree[i][position].inputs.Length == 0)
                            rowThree[i][position].inputs = new int[] { position };
                        else if (rowThree[i][position].inputs.Length > 2)
                        {
                            int[] inputs = rowThree[i][position].inputs;
                            rowThree[i][position].inputs = new int[] { inputs[0], inputs[1] };
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
                            rowThree[i][position].inputs[0] = gateLengths[row] - 1;
                    }
                }
            }
        }
    }
    private void dealWithPermanentGates()
    {
        for (int i = 0; i < permanentGates.Length;i++)
        {
            for (int j = 0; j < permanentGates[i].Length; j++)
            {
                for (int k = 0; k < permanentGates[i][j].Length; k++)
                {
                    if (permanentGates[i][j][k] == true)
                    {
                        string type = "";
                        int inputs = 0;
                        switch (j)
                        {
                            case 0:
                                type = rowOne[i][k].type;
                                inputs = rowOne[i][k].inputs.Length;
                                break;
                            case 1:
                                type = rowTwo[i][k].type;
                                inputs = rowTwo[i][k].inputs.Length;
                                break;
                            case 2:
                                type = rowThree[i][k].type;
                                inputs = rowThree[i][k].inputs.Length;
                                break;
                        }
                        if ((type == "buffer" || type == "not") && inputs > 1)
                        {
                            switch (j)
                            {
                                case 0:
                                    rowOne[i][k].inputs = new int[] { rowOne[i][k].inputs[0] };
                                    break;
                                case 1:
                                    rowTwo[i][k].inputs = new int[] { rowTwo[i][k].inputs[0] };
                                    break;
                                case 2:
                                    rowThree[i][k].inputs = new int[] { rowThree[i][k].inputs[0] };
                                    break;
                            }
                        }

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

    public void resetLogicColor()
    {
        foreach (GameObject button in buttons)
        {
            button.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        foreach (GameObject line in lines)
        {
            line.GetComponent<LineRenderer>().startColor = new Color32(60, 60, 60, 255);
            line.GetComponent<LineRenderer>().endColor = new Color32(60, 60, 60, 255);
        }
    }
    public void colorInput(int position, bool on)
    {
        if (on)
            buttons[position].GetComponent<Image>().color = new Color32(0, 255, 0, 255);
        else
            buttons[position].GetComponent<Image>().color = new Color32(255, 0, 0, 255);
    }
    public void colorLogic(int row, int position, bool on, bool[] inputLines)
    {
        int buttonsInRowsAbove = inputs[currentField].Length;
        int inputsInRowsAbove = 0;
        int inputsInRow = 0;
        for (int i = 0; i < row - 1; i++)
        {
            switch (i)
            {
                case 0:
                    buttonsInRowsAbove += rowOne[currentField].Length;
                    foreach (LogicGate gate in rowOne[currentField])
                        inputsInRowsAbove += gate.inputs.Length;
                    break;
                case 1:
                    buttonsInRowsAbove += rowTwo[currentField].Length;
                    foreach (LogicGate gate in rowTwo[currentField])
                        inputsInRowsAbove += gate.inputs.Length;
                    break;
                case 2:
                    buttonsInRowsAbove += rowThree[currentField].Length;
                    foreach (LogicGate gate in rowThree[currentField])
                        inputsInRowsAbove += gate.inputs.Length;
                    break;
            }
        }
        switch (row)
        {
            case 1:
                for (int i = 0; i < position; i++)
                    inputsInRow += rowOne[currentField][i].inputs.Length;
                break;
            case 2:
                for (int i = 0; i < position; i++)
                    inputsInRow += rowTwo[currentField][i].inputs.Length;
                break;
            case 3:
                for (int i = 0; i < position; i++)
                    inputsInRow += rowThree[currentField][i].inputs.Length;
                break;
            default:
                Debug.Log("Error");
                break;
        }
        if (on)
            buttons[position + buttonsInRowsAbove].GetComponent<Image>().color = new Color32(0, 255, 0, 255);
        else
            buttons[position + buttonsInRowsAbove].GetComponent<Image>().color = new Color32(255, 0, 0, 255);
        for (int i = 0; i < inputLines.Length; i++)
        {
            if (inputLines[i])
            {
                lines[i + inputsInRowsAbove + inputsInRow].GetComponent<LineRenderer>().startColor = new Color32(0, 255, 0, 255);
                lines[i + inputsInRowsAbove + inputsInRow].GetComponent<LineRenderer>().endColor = new Color32(0, 255, 0, 255);
            }
            else
            {
                lines[i + inputsInRowsAbove + inputsInRow].GetComponent<LineRenderer>().startColor = new Color32(255, 0, 0, 255);
                lines[i + inputsInRowsAbove + inputsInRow].GetComponent<LineRenderer>().endColor = new Color32(255, 0, 0, 255);
            }
        }
    }
    public void colorOutput(int position, bool on, bool inputLine)
    {
        int buttonsInRowsAbove = inputs[currentField].Length;
        buttonsInRowsAbove += rowOne[currentField].Length;
        buttonsInRowsAbove += rowTwo[currentField].Length;
        buttonsInRowsAbove += rowThree[currentField].Length;
        if (on)
            buttons[position + buttonsInRowsAbove].GetComponent<Image>().color = new Color32(0, 255, 0, 255);
        else
            buttons[position + buttonsInRowsAbove].GetComponent<Image>().color = new Color32(255, 0, 0, 255);
        int inputsInRowsAbove = 0;
        foreach (LogicGate gate in rowOne[currentField])
            inputsInRowsAbove += gate.inputs.Length;
        foreach (LogicGate gate in rowTwo[currentField])
            inputsInRowsAbove += gate.inputs.Length;
        foreach (LogicGate gate in rowThree[currentField])
            inputsInRowsAbove += gate.inputs.Length;
        if (inputLine)
        {
            lines[position + inputsInRowsAbove].GetComponent<LineRenderer>().startColor = new Color32(0, 255, 0, 255);
            lines[position + inputsInRowsAbove].GetComponent<LineRenderer>().endColor = new Color32(0, 255, 0, 255);
        }
        else
        {
            lines[position + inputsInRowsAbove].GetComponent<LineRenderer>().startColor = new Color32(255, 0, 0, 255);
            lines[position + inputsInRowsAbove].GetComponent<LineRenderer>().endColor = new Color32(255, 0, 0, 255);
        }
    }

    public void debugLog()
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            Debug.Log("Field: " + i);
            Debug.Log("Inputs: ");
            for (int j = 0; j < inputs[i].Length; j++)
            {
                Debug.Log("Input " + j + ": " + inputs[i][j].type);
            }
            Debug.Log("RowOne: ");
            for (int j = 0; j < rowOne[i].Length; j++)
            {
                Debug.Log("Gate " + j + ": " + rowOne[i][j].type + " " + rowOne[i][j].inputs.Length);
            }
            Debug.Log("RowTwo: ");
            for (int j = 0; j < rowTwo[i].Length; j++)
            {
                Debug.Log("Gate " + j + ": " + rowTwo[i][j].type + " " + rowTwo[i][j].inputs.Length);
            }
            Debug.Log("RowThree: ");
            for (int j = 0; j < rowThree[i].Length; j++)
            {
                Debug.Log("Gate " + j + ": " + rowThree[i][j].type + " " + rowThree[i][j].inputs.Length);
            }
            Debug.Log("Outputs: ");
            for (int j = 0; j < outputs[i].Length; j++)
            {
                Debug.Log("Output " + j + ": " + outputs[i][j].type + " " + outputs[i][j].inputs.Length);
            }
        }
    }

    public void selectButton()
    {
        if (buttons == null || buttons.Length == 0)
            return;
        if (clickedButton[0] < 0 && clickedButton[1] < 0)
        {
            buttons[0].GetComponent<Button>().Select();
        }
        else if (clickedButton[0] == 0)
        {
            buttons[clickedButton[1]].GetComponent<Button>().Select();
        }
        else
        {
            int numberOfRows = 0;
            numberOfRows += (rowOne[currentField].Length > 0) ? 1 : 0;
            numberOfRows += (rowTwo[currentField].Length > 0) ? 1 : 0;
            numberOfRows += (rowThree[currentField].Length > 0) ? 1 : 0;
            int numberOfButtonsBefore = inputs[currentField].Length;
            for (int i = 1; i < clickedButton[0]; i++)
            {
                switch (i)
                {
                    case 1:
                        numberOfButtonsBefore += rowOne[currentField].Length;
                        break;
                    case 2:
                        numberOfButtonsBefore += rowTwo[currentField].Length;
                        break;
                    case 3:
                        numberOfButtonsBefore += rowThree[currentField].Length;
                        break;
                    default:
                        Debug.Log("Error");
                        break;
                }
            }
            numberOfButtonsBefore += clickedButton[1];
            buttons[numberOfButtonsBefore].GetComponent<Button>().Select();
        }
        clickedButton[0] = -1;
        clickedButton[1] = -1;
    }
    public void setChooseButtonsActive(bool active)
    {
        chooseButtonActive = active;
    }

    private void Update()
    {
        if (!play.GetComponent<Play>().getIsPlay() && chooseButtonActive)
        {
            if (Input.GetKeyDown("joystick button 4"))
            {
                if (currentField > 0)
                {
                    onClickChoose(currentField - 1);
                }
            }
            if (Input.GetKeyDown("joystick button 5"))
            {
                if (currentField < chooseButtons.Length - 1)
                {
                    onClickChoose(currentField + 1);
                }
            }
        }
    }
}
