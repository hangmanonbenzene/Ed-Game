using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Play : MonoBehaviour
{
    [SerializeField] private GameObject playLevel;
    [SerializeField] private GameObject screenHolder;

    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject field;
    [SerializeField] private GameObject disableLogic;
    [SerializeField] private GameObject levelLogic;
    [SerializeField] private GameObject gameLogic;
    [SerializeField] private GameObject won;
    [SerializeField] private GameObject lines;

    private bool isPlay = false;
    private int wonPlayers;

    private readonly float time = 0.5f;

    public void onClickPlay()
    {
        if (LevelName.getLevelName() == "")
        {
            return;
        }
        if (!isPlay)
        {
            disableLogic.SetActive(true);
            isPlay = true;
            playButton.GetComponentInChildren<TextMeshProUGUI>().text = "Stop";
            StartCoroutine(play());
        }
        else
        {
            GetComponent<Button>().interactable = false;
            isPlay = false;
            playButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start";
        }
    }

    private IEnumerator play()
    {
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            if (isPlay)
                yield return null;
            else
                break;
        }

        int[,] player = field.GetComponent<LevelField>().getPlayerPositions();
        int players = player.GetLength(0);
        int currentPlayer = 0;
        int logicFields = levelLogic.GetComponent<LevelLogic>().GetOutputs().GetLength(0);
        int currentField = 0;
        int outputsInField = levelLogic.GetComponent<LevelLogic>().GetOutputs()[currentField].GetLength(0);
        int currentPosition = 0;
        wonPlayers = players;

        while (isPlay)
        {
            if (players != 0)
            {
                levelLogic.GetComponent<LevelLogic>().onClickChoose(currentField);
                yield return null;
                levelLogic.GetComponent<LevelLogic>().resetLogicColor();
                gameLogic.GetComponent<Outputs>().use(
                    new int[,] { { player[currentPlayer, 0], player[currentPlayer, 1], player[currentPlayer, 2] } }, 
                    currentPosition, currentField);
                if (currentPlayer == players - 1)
                {
                    if (currentPosition == outputsInField - 1)
                    {
                        if (currentField == logicFields - 1)
                        {
                            currentField = 0;
                        }
                        else
                        {
                            currentField++;
                        }
                        outputsInField = levelLogic.GetComponent<LevelLogic>().GetOutputs()[currentField].GetLength(0);
                        currentPosition = 0;
                    }
                    else
                    {
                        currentPosition++;
                    }
                    player = field.GetComponent<LevelField>().getPlayerPositions();
                    players = player.GetLength(0);
                    currentPlayer = 0;
                }
                else
                {
                    currentPlayer++;
                }
            }
            for (float i = 0; i < time; i += Time.deltaTime)
            {
                if(isPlay)
                    yield return null;
                else
                    break;
            }
            if (wonPlayers == 0)
            {
                lines.SetActive(false);
                if (LevelName.getStoryMode())
                {
                    int level = StorySettings.getScreenNumbers(SaveSystem.getStory(LevelName.getLevelName()).Level)[1];
                    Instantiate(playLevel.GetComponent<PlayLevel>().getPostScreen(level), screenHolder.transform);
                }
                else
                    won.SetActive(true);
                onClickPlay();
            }
        }

        levelLogic.GetComponent<LevelLogic>().resetLogicColor();
        field.GetComponent<LevelField>().resetField();
        gameLogic.GetComponent<Inputs>().resetMemory();
        GetComponent<Button>().interactable = true;
        disableLogic.SetActive(false);
    }

    public void wonPlayer()
    {
        wonPlayers--;
    }
    public bool getIsPlay()
    {
        return isPlay;
    }

    public void onClickBackToLevel()
    {
        won.SetActive(false);
        lines.SetActive(true);
    }
}
