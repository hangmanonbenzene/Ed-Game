using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoosePrompt : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;

    private GameObject[][] buttons;
    [SerializeField] private GameObject buttonholder;
    [SerializeField] private GameObject doneButton;
    [SerializeField] private GameObject prompt;

    private int[] selectedButtons;
    private bool[] selectedInputs;
    private int selectedType;
    private GameObject initButton;

    public void setup()
    {
        buttons = new GameObject[4][];
        buttons[0] = new GameObject[8];
        buttons[1] = new GameObject[8];
        buttons[2] = new GameObject[8];
        buttons[3] = new GameObject[6];
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < buttons[i].Length; j++)
            {
                buttons[i][j] = Instantiate(buttonPrefab, new Vector3(0, 0, 0), new Quaternion(), buttonholder.transform);
                buttons[i][j].GetComponent<RectTransform>().anchoredPosition = new Vector2((float)(180 * (j - 3.5)), (float)(180 * (-i + 1.5)));
                if (i == 0)
                {
                    // Set the font size of the type buttons
                    buttons[i][j].GetComponentInChildren<TextMeshProUGUI>().fontSize = 48;
                }
            }
        }

        selectedButtons = new int[4];
        selectedInputs = new bool[5];
    }

    public void activateInput(GameObject button, int typeNumber, int specificationOne, int specificationTwo, int specificationThree)
    {
        selectedType = 0;
        initButton = button;

        string type = TypesOfInputs.getTypes()[typeNumber];
        setTypeInput(type);

        select(0, typeNumber);
        select(1, specificationOne);
        select(2, specificationTwo);
        select(3, specificationThree);

        selectedButtons[0] = typeNumber;
        selectedButtons[1] = specificationOne;
        selectedButtons[2] = specificationTwo;
        selectedButtons[3] = specificationThree;

        for (int i = 0; i < buttons.Length; i++)
        {
            for (int j = 0; j < buttons[i].Length; j++)
            {
                int row = i;
                int number = j;
                if (row == 0)
                {
                    buttons[row][j].GetComponent<Button>().onClick.RemoveAllListeners();
                    buttons[row][j].GetComponent<Button>().onClick.AddListener(() => onClickType(number));
                }
                else
                {
                    buttons[row][j].GetComponent<Button>().onClick.RemoveAllListeners();
                    buttons[row][j].GetComponent<Button>().onClick.AddListener(() => onClickSpecification(row, number));
                }
            }
        }

        doneButton.GetComponent<Button>().onClick.RemoveAllListeners();
        doneButton.GetComponent<Button>().onClick.AddListener(onClickDoneInput);

        prompt.SetActive(true);
    }
    public void activateGate(GameObject button, int typeNumber, int[] inputs)
    {
        selectedType = 1;
        initButton = button;

        string type = TypesOfLogic.getTypes()[typeNumber];
        setTypeGate(type);

        select(0, typeNumber);
        for (int i = 0; i < 5; i++)
        {
            if (inputs.Contains(i))
                buttons[1][i].GetComponent<TypeButton>().select(true);
            else
                buttons[1][i].GetComponent<TypeButton>().select(false);
        }

        selectedButtons[0] = typeNumber;
        selectedInputs[0] = inputs.Contains(0);
        selectedInputs[1] = inputs.Contains(1);
        selectedInputs[2] = inputs.Contains(2);
        selectedInputs[3] = inputs.Contains(3);
        selectedInputs[4] = inputs.Contains(4);

        for (int i = 0; i < buttons.Length; i++)
        {
            for (int j = 0; j < buttons[i].Length; j++)
            {
                int row = i;
                int number = j;
                if (row == 0)
                {
                    buttons[row][j].GetComponent<Button>().onClick.RemoveAllListeners();
                    buttons[row][j].GetComponent<Button>().onClick.AddListener(() => onClickType(number));
                }
                else if (row == 1)
                {
                    buttons[row][j].GetComponent<Button>().onClick.RemoveAllListeners();
                    buttons[row][j].GetComponent<Button>().onClick.AddListener(() => onClickInput(1, number));
                }
                else
                {
                    buttons[row][j].GetComponent<Button>().onClick.RemoveAllListeners();
                }
            }
        }

        doneButton.GetComponent<Button>().onClick.RemoveAllListeners();
        doneButton.GetComponent<Button>().onClick.AddListener(onClickDoneGate);

        prompt.SetActive(true);
    }
    public void activateOutput(GameObject button, int typeNumber, int specification, int[] inputs)
    {
        selectedType = 2;
        initButton = button;

        string type = TypesOfOutputs.getTypes()[typeNumber];
        setTypeOutput(type);

        select(0, typeNumber);
        select(1, specification);
        for (int i = 0; i < 5; i++)
        {
            if (inputs.Contains(i))
                buttons[2][i].GetComponent<TypeButton>().select(true);
            else
                buttons[2][i].GetComponent<TypeButton>().select(false);
        }

        selectedButtons[0] = typeNumber;
        selectedButtons[1] = specification;
        selectedInputs[0] = inputs.Contains(0);
        selectedInputs[1] = inputs.Contains(1);
        selectedInputs[2] = inputs.Contains(2);
        selectedInputs[3] = inputs.Contains(3);
        selectedInputs[4] = inputs.Contains(4);

        for (int i = 0; i < buttons.Length; i++)
        {
            for (int j = 0; j < buttons[i].Length; j++)
            {
                int row = i;
                int number = j;
                if (row == 0)
                {
                    buttons[row][j].GetComponent<Button>().onClick.RemoveAllListeners();
                    buttons[row][j].GetComponent<Button>().onClick.AddListener(() => onClickType(number));
                }
                else if (row == 1)
                {
                    buttons[row][j].GetComponent<Button>().onClick.RemoveAllListeners();
                    buttons[row][j].GetComponent<Button>().onClick.AddListener(() => onClickSpecification(row, number));
                }
                else if (row == 2)
                {
                    buttons[row][j].GetComponent<Button>().onClick.RemoveAllListeners();
                    buttons[row][j].GetComponent<Button>().onClick.AddListener(() => onClickInput(2, number));
                }
                else
                {
                    buttons[row][j].GetComponent<Button>().onClick.RemoveAllListeners();
                }
            }
        }

        doneButton.GetComponent<Button>().onClick.RemoveAllListeners();
        doneButton.GetComponent<Button>().onClick.AddListener(onClickDoneOutput);

        prompt.SetActive(true);
    }

    public void onClickType(int number)
    {
        if (selectedButtons[0] == number)
        {
            return;
        }
        select(0, number);
        selectedButtons[0] = number;
        switch (selectedType)
        {
            case 0:
                setTypeInput(TypesOfInputs.getTypes()[number]);
                select(1, 0);
                selectedButtons[1] = 0;
                select(2, 0);
                selectedButtons[2] = 0;
                select(3, 0);
                selectedButtons[3] = 0;
                break;
            case 1:
                setTypeGate(TypesOfLogic.getTypes()[number]);
                break;
            case 2:
                setTypeOutput(TypesOfOutputs.getTypes()[number]);
                select(1, 0);
                selectedButtons[1] = 0;
                break;
        }
    }
    public void onClickSpecification(int row, int number)
    {
        selectedButtons[row] = number;
        select(row, number);
    }
    public void onClickInput(int row, int number)
    {
        if (buttons[row][number].GetComponent<TypeButton>().isSelected())
        {
            buttons[row][number].GetComponent<TypeButton>().select(false);
            selectedInputs[number] = false;
        }
        else
        {
            buttons[row][number].GetComponent<TypeButton>().select(true);
            selectedInputs[number] = true;
        }
    }
    public void select(int row, int number)
    {
        for (int i = 0; i < buttons[row].Length; i++)
        {
            if (i == number)
                buttons[row][i].GetComponent<TypeButton>().select(true);
            else
                buttons[row][i].GetComponent<TypeButton>().select(false);
        }
    }

    private void onClickDoneInput()
    {
        initButton.GetComponent<FieldInput>().onButtonChange(selectedButtons[0], selectedButtons[1], selectedButtons[2], selectedButtons[3]);

        prompt.SetActive(false);
    }
    private void onClickDoneGate()
    {
        int number = 0;
        for (int i = 0; i < 5; i++)
        {
            if (selectedInputs[i])
                number++;
        }
        int[] inputs = new int[number];
        int index = 0;
        for (int i = 0; i < 5; i++)
        {
            if (selectedInputs[i])
            {
                inputs[index] = i;
                index++;
            }
        }
        initButton.GetComponent<FieldLogic>().onButtonChange(selectedButtons[0], inputs);

        prompt.SetActive(false);
    }
    private void onClickDoneOutput()
    {
        int number = 0;
        for (int i = 0; i < 5; i++)
        {
            if (selectedInputs[i])
                number++;
        }
        int[] inputs = new int[number];
        int index = 0;
        for (int i = 0; i < 5; i++)
        {
            if (selectedInputs[i])
            {
                inputs[index] = i;
                index++;
            }
        }
        initButton.GetComponent<FieldOutput>().onButtonChange(selectedButtons[0], selectedButtons[1], inputs);

        prompt.SetActive(false);
    }

    private void activateButtons(int row, int count)
    {
          for (int i = 0; i < buttons[row].Length; i++)
        {
            if (i < count)
                buttons[row][i].SetActive(true);
            else
                buttons[row][i].SetActive(false);
        }
    }

    private void setTypeInput(string type)
    {
        activateButtons(0, TypesOfInputs.getTypes().Length);
        for (int i = 0; i < TypesOfInputs.getTypes().Length; i++)
        {
            buttons[0][i].GetComponent<TypeButton>().setType(TypesOfInputs.getTypes()[i], TypesOfInputs.getTypes()[i]);
        }
        for (int i = 1; i < 4; i++)
        {
            int count = TypesOfInputs.getSpecificationsForType(type, i).Length;
            activateButtons(i, count);
            for (int j = 0; j < count; j++)
            {
                buttons[i][j].GetComponent<TypeButton>().setType(TypesOfInputs.getSpecificationsForType(type, i)[j], TypesOfInputs.getSymbolForType(type, i, j));
            }
        }
    }
    private void setTypeGate(string type)
    {
        activateButtons(0, TypesOfLogic.getTypes().Length);
        for (int i = 0; i < TypesOfLogic.getTypes().Length; i++)
        {
            buttons[0][i].GetComponent<TypeButton>().setType(TypesOfLogic.getTypes()[i], TypesOfLogic.getTypes()[i]);
        }
        activateButtons(1, 5);
        activateButtons(2, 0);
        activateButtons(3, 0);
        for (int i = 0; i < 5; i++)
        {
            buttons[1][i].GetComponent<TypeButton>().setType("input", "");
        }
    }
    private void setTypeOutput(string type)
    {
        activateButtons(0, TypesOfOutputs.getTypes().Length);
        for (int i = 0; i < TypesOfOutputs.getTypes().Length; i++)
        {
            buttons[0][i].GetComponent<TypeButton>().setType(TypesOfOutputs.getTypes()[i], TypesOfOutputs.getTypes()[i]);
        }
        int count = TypesOfOutputs.getSpecificationsForType(type).Length;
        activateButtons(1, count);
        activateButtons(2, 5);
        activateButtons(3, 0);
        for (int i = 0; i < count; i++)
        {
            buttons[1][i].GetComponent<TypeButton>().setType(TypesOfOutputs.getSpecificationsForType(type)[i], TypesOfOutputs.getSymbolForType(type, i));
        }
        for (int i = 0; i < 5; i++)
        {
            buttons[2][i].GetComponent<TypeButton>().setType("input", "");
        }
    }
}
