using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LogicPrompt : MonoBehaviour
{
    [SerializeField] private GameObject levelLogic;
    [SerializeField] private GameObject lines;

    [SerializeField] private GameObject prompt;
    [SerializeField] private GameObject titel;
    [SerializeField] private GameObject text;
    [SerializeField] private GameObject[] buttons;

    private string type;
    private LogicGate logic;
    private int[] activeButtons;

    private void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int button = i;
            buttons[i].GetComponent<Button>().onClick.AddListener(() => selectButton(button));
        }
    }
    public void setInput(SensorInput input)
    {
        type = "input";
        titel.GetComponent<TMPro.TextMeshProUGUI>().text = "Input";
        setInputText(input.type, input.specificationOne, input.specificationTwo, input.specificationThree);
        foreach (GameObject button in buttons)
        {
            button.GetComponent<Button>().interactable = false;
            button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "";
        }
        selectButton(-1);
        activatePrompt();
    }
    public void setLogic(LogicGate logic, bool multipleInputs, int[] restrictedGates)
    {
        type = "logic";
        this.logic = logic;
        titel.GetComponent<TMPro.TextMeshProUGUI>().text = "Logik-Gatter";
        setLogicText(logic.type);
        activeButtons = multipleInputs ? new int[] { 0, 1, 2, 3, 5, 6, 7 } : new int[] { 0, 4 };
        buttons[0].GetComponent<Button>().interactable = !multipleInputs;
        buttons[0].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "";
        buttons[1].GetComponent<Button>().interactable = multipleInputs;
        buttons[1].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = multipleInputs ? "AND" : "";
        buttons[2].GetComponent<Button>().interactable = multipleInputs;
        buttons[2].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = multipleInputs ? "OR" : "";
        buttons[3].GetComponent<Button>().interactable = multipleInputs;
        buttons[3].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = multipleInputs ? "XOR" : "";
        buttons[4].GetComponent<Button>().interactable = !multipleInputs;
        buttons[4].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = multipleInputs ? "" : "NOT";
        buttons[5].GetComponent<Button>().interactable = multipleInputs;
        buttons[5].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = multipleInputs ? "NAND" : "";
        buttons[6].GetComponent<Button>().interactable = multipleInputs;
        buttons[6].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = multipleInputs ? "NOR" : "";
        buttons[7].GetComponent<Button>().interactable = multipleInputs;
        buttons[7].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = multipleInputs ? "XNOR" : "";

        if (restrictedGates != null)
        {
            foreach (int i in restrictedGates)
            {
                buttons[i].GetComponent<Button>().interactable = false;
                buttons[i].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "";
            }
        }

        switch (logic.type)
        {
            case "buffer":
                selectButton(0);
                break;
            case "and":
                selectButton(1);
                break;
            case "or":
                selectButton(2);
                break;
            case "xor":
                selectButton(3);
                break;
            case "not":
                selectButton(4);
                break;
            case "nand":
                selectButton(5);
                break;
            case "nor":
                selectButton(6);
                break;
            case "xnor":
                selectButton(7);
                break;
            default:
                selectButton(-1);
                break;
        }

        activatePrompt();
    }
    public void setOutput(SensorOutput output)
    {
        type = "output";
        titel.GetComponent<TMPro.TextMeshProUGUI>().text = "Output";
        setOutputText(output.type, output.specification);
        foreach (GameObject button in buttons)
        {
            button.GetComponent<Button>().interactable = false;
            button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "";
        }
        selectButton(-1);
        activatePrompt();
    }

    private void selectButton(int button)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == button)
            {
                buttons[i].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                buttons[i].GetComponentInChildren<TMPro.TextMeshProUGUI>().color = new Color(1f, 1f, 1f);
            }
            else
            {
                buttons[i].GetComponent<Image>().color = new Color(1f, 1f, 1f);
                buttons[i].GetComponentInChildren<TMPro.TextMeshProUGUI>().color = new Color32(50, 50, 50, 255);
            }
        }
        if (button >= 0)
        {
            logic.type = TypesOfLogic.getTypes()[button + 1];
            setLogicText(logic.type);
        }
    }
    private void activatePrompt()
    {
        lines.SetActive(false);
        prompt.SetActive(true);
    }
    public void onCLickDone()
    {         
        if (type == "logic")
        {
            levelLogic.GetComponent<LevelLogic>().setLogic(logic);
        }
        prompt.SetActive(false);
        lines.SetActive(true);
    }

    private void setInputText(string type, string spec1, string spec2, string spec3)
    {
        switch (type)
        {
            case "false":
                text.GetComponent<TMPro.TextMeshProUGUI>().text = "Ist immer <color=#FF0000>falsch</color>.";
                break;
            case "true":
                text.GetComponent<TMPro.TextMeshProUGUI>().text = "Ist immer <color=#00FF00>wahr</color>.";
                break;
            case "look":
                string[] specs = getInputSpecText(type, spec1, spec2, spec3);
                text.GetComponent<TMPro.TextMeshProUGUI>().text = "Ist <color=#00FF00>wahr</color>, wenn der Spieler " + specs[0] + " sich " + specs[1] + " in einer Entfernung von höchstens " + specs[2] + " sieht.";
                break;
            case "read":
                string spec = getInputSpecText(type, spec1, spec2, spec3)[0];
                text.GetComponent<TMPro.TextMeshProUGUI>().text = "Gibt den Wert der <color=#0000FF>Speicherzelle</color> " + spec + " zurück.<br>Speicherzellen beginnen mit dem Wert <color=#FF0000>falsch</color>.";
                break;
        }
    }
    private void setLogicText(string type)
    {
        switch (logic.type)
        {
            case "buffer":
                text.GetComponent<TMPro.TextMeshProUGUI>().text = "Gibt den Wert des <color=#0000FF>Inputs</color> zurück.";
                break;
            case "and":
                text.GetComponent<TMPro.TextMeshProUGUI>().text = "Gibt <color=#00FF00>wahr</color> zurück, wenn <color=#0000FF>alle</color> Inputs <color=#00FF00>wahr</color> sind.";
                break;
            case "or":
                text.GetComponent<TMPro.TextMeshProUGUI>().text = "Gibt <color=#00FF00>wahr</color> zurück, wenn <color=#0000FF>mindestens ein</color> Input <color=#00FF00>wahr</color> ist.";
                break;
            case "xor":
                text.GetComponent<TMPro.TextMeshProUGUI>().text = "Gibt <color=#00FF00>wahr</color> zurück, wenn <color=#0000FF>genau ein</color> Input <color=#00FF00>wahr</color> ist.";
                break;
            case "not":
                text.GetComponent<TMPro.TextMeshProUGUI>().text = "Gibt den <color=#0000FF>umgekehrten</color> Wert des <color=#0000FF>Inputs</color> zurück.";
                break;
            case "nand":
                text.GetComponent<TMPro.TextMeshProUGUI>().text = "Gibt <color=#00FF00>wahr</color> zurück, wenn <color=#0000FF>mindestens ein</color> Input <color=#FF0000>falsch</color> ist.";
                break;
            case "nor":
                text.GetComponent<TMPro.TextMeshProUGUI>().text = "Gibt <color=#00FF00>wahr</color> zurück, wenn <color=#0000FF>alle</color> Inputs <color=#FF0000>falsch</color> sind.";
                break;
            case "xnor":
                text.GetComponent<TMPro.TextMeshProUGUI>().text = "Gibt <color=#00FF00>wahr</color> zurück, wenn <color=#0000FF>alle</color> Inputs <color=#0000FF>gleich</color> sind.";
                break;
            default:
                text.GetComponent<TMPro.TextMeshProUGUI>().text = "Wähle ein Logik-Gatter aus.";
                break;
        }
    }
    private void setOutputText(string type, string spec)
    {
        switch (type)
        {
            case "wait":
                text.GetComponent<TMPro.TextMeshProUGUI>().text = "Der Spieler macht <color=#0000FF>nichts</color>.";
                break;
            case "walk":
                text.GetComponent<TMPro.TextMeshProUGUI>().text = "Der Spieler <color=#0000FF>läuft</color> nach <color=#0000FF>vorne</color>.";
                break;
            case "turn":
                text.GetComponent<TMPro.TextMeshProUGUI>().text = "Der Spieler <color=#0000FF>dreht</color> sich nach <color=#0000FF>" + (spec == "left" ? "links" : "rechts") + "</color>.";
                break;
            case "write":
                text.GetComponent<TMPro.TextMeshProUGUI>().text = "<color=#0000FF>Schreibt</color> den Wert des <color=#0000FF>Inputs</color> in die <color=#0000FF>Speicherzelle " + spec + "</color>.<br>Speicherzellen beginnen mit dem Wert <color=#FF0000>falsch</color>.";
                break;
        }
    }

    private string[] getInputSpecText(string type, string spec1, string spec2, string spec3)
    {
        string[] text = new string[3];
        switch (type)
        {
            case "look":
                text[0] = spec1 switch
                {
                    "forwards" => "<color=#0000FF>vor</color>",
                    "left" => "<color=#0000FF>links</color> neben",
                    "backwards" => "<color=#0000FF>hinter</color>",
                    "right" => "<color=#0000FF>rechts</color> neben",
                    _ => "-",
                };
                text[1] = spec2 switch
                {
                    "empty" => "ein <color=#0000FF>leeres Feld</color>",
                    "wall" => "eine <color=#0000FF>Wand</color>",
                    "player" => "einen <color=#0000FF>Spieler</color>",
                    "goal" => "ein <color=#0000FF>Ziel</color>",
                    "hole" => "ein <color=#0000FF>Loch</color>",
                    _ => "-",
                };
                text[2] = spec3 switch
                {
                    "one" => "<color=#0000FF>einem Feld</color>",
                    "two" => "<color=#0000FF>zwei Feldern</color>",
                    "three" => "<color=#0000FF>drei Feldern</color>",
                    "four" => "<color=#0000FF>vier Feldern</color>",
                    _ => "-",
                };
                break;
            case "read":
                text[0] = spec1 switch
                {
                    "A" => "<color=#0000FF>A</color>",
                    "B" => "<color=#0000FF>B</color>",
                    "C" => "<color=#0000FF>C</color>",
                    "D" => "<color=#0000FF>D</color>",
                    _ => "-",
                };
                break;
        }
        return text;
    }
}
