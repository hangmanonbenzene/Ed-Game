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
    [SerializeField] private GameObject pauseButton;

    private bool isPlay = false;
    private bool isPause = false;
    private bool isWon = false;
    private int wonPlayers;

    private float time;

    public void onClickPlay()
    {
        if (LevelName.getLevelName() == "")
        {
            return;
        }
        if (!isPlay)
        {
            pauseButton.GetComponent<Button>().interactable = true;
            disableLogic.SetActive(true);
            isPlay = true;
            playButton.GetComponentInChildren<TextMeshProUGUI>().text = "Stop";
            StartCoroutine(play());
        }
        else
        {
            pauseButton.GetComponent<Button>().interactable = false;
            pauseButton.GetComponentInChildren<TextMeshProUGUI>().text = "Pause";
            isPause = false;
            GetComponent<Button>().interactable = false;
            isPlay = false;
            playButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start";
        }
    }
    public void onCLickPause()
    {
        if (!isPause)
        {
            isPause = true;
            pauseButton.GetComponentInChildren<TextMeshProUGUI>().text = "Weiter";
        }
        else
        {
            isPause = false;
            pauseButton.GetComponentInChildren<TextMeshProUGUI>().text = "Pause";
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

        int[,] player = field.GetComponent<LevelField>().getAll("player");
        int players = player.GetLength(0);
        int currentPlayer = 0;
        int logicFields = levelLogic.GetComponent<LevelLogic>().GetOutputs().GetLength(0);
        int currentField = 0;
        int outputsInField = levelLogic.GetComponent<LevelLogic>().GetOutputs()[currentField].GetLength(0);
        int currentPosition = 0;
        wonPlayers = players;
        isWon = false;

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

                int[,] mummies = field.GetComponent<LevelField>().getAll("mummy");
                for (int i = 0; i < mummies.GetLength(0); i++)
                    gameLogic.GetComponent<Outputs>().walkMummy(new int[,] { { mummies[i, 0], mummies[i, 1], mummies[i, 2] } });

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
                    player = field.GetComponent<LevelField>().getAll("player");
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
                if(isPlay || isWon)
                    yield return null;
                else
                    break;
            }
            if (isWon)
            {
                lines.SetActive(false);
                if (LevelName.getStoryMode())
                {
                    int level = StorySettings.getScreenNumbers(SaveSystem.getStory(LevelName.getLevelName()).Level)[1];
                    Instantiate(playLevel.GetComponent<PlayLevel>().getPostScreen(level), screenHolder.transform);
                }
                else
                    won.SetActive(true);
            }
            while (isPause)
            {
                yield return null;
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
        if (wonPlayers == 0)
        {
            isWon = true;
        }
        onClickPlay();
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
    public void setTime(float time)
    {
        this.time = time;
    }
}
