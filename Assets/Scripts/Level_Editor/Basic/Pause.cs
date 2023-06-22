using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject levelEditor;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject nameInput;
    [SerializeField] private GameObject saveGameCreator;

    public void onClickPause()
    {
        pauseMenu.SetActive(true);
        nameInput.GetComponent<TMPro.TMP_InputField>().text = saveGameCreator.GetComponent<LevelEditor>().getLevelName();
    }

    public void onClickResume()
    {
        pauseMenu.SetActive(false);
    }
    public void onClickSaveAndQuit()
    {
        string levelName = nameInput.GetComponent<TMPro.TMP_InputField>().text;
        if (levelName.Equals("Neues Level") || levelName.Equals(""))
        {
            Debug.Log("Error: Level name is invalid");
        }
        else
        {
            LevelData levelData = saveGameCreator.GetComponent<SaveGameCreator>().createSaveGame(levelName);
            SaveSystem.saveLevel(levelData);

            onClickQuit();
        }
    }
    public void onClickSaveAndPlay()
    {
        string levelName = nameInput.GetComponent<TMPro.TMP_InputField>().text;
        if (levelName.Equals("Neues Level") || levelName.Equals(""))
        {
            Debug.Log("Error: Level name is invalid");
        }
        else
        {
            LevelData levelData = saveGameCreator.GetComponent<SaveGameCreator>().createSaveGame(levelName);
            SaveSystem.saveLevel(levelData);
            
            LevelName.setLevelName(levelName);

            SceneManager.LoadScene(1);
        }
    }
    public void onClickQuit()
    {
        LevelName.setMenu("Editor");
        SceneManager.LoadScene(0);
    }
}
