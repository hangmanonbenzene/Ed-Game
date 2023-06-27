using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryScreen : MonoBehaviour
{
    [SerializeField] private GameObject[] screens;
    private int currentScreen;
    private GameObject currentScreenObject;
    [SerializeField] private GameObject screenHolder;

    private void Start()
    {
        if (screens != null && screens.Length > 0)
        {
            currentScreen = 0;
            currentScreenObject = Instantiate(screens[currentScreen], screenHolder.transform);
            gameObject.GetComponentInChildren<Button>().Select();
        }
        else
        {
            FindObjectOfType<Canvas>().gameObject.GetComponent<PlayLevel>().enableButtons();
            FindObjectOfType<Canvas>().gameObject.GetComponent<PlayLevel>().select();
            FindObjectOfType<Canvas>().gameObject.GetComponent<PlayLevel>().linesActive(true);
            Debug.Log("No screens found");
            Destroy(gameObject);
        }
    }

    public void onClickDone()
    {
        currentScreen++;
        if (currentScreen < screens.Length)
        {
            Destroy(currentScreenObject);
            currentScreenObject = Instantiate(screens[currentScreen], screenHolder.transform);
        }
        else
        {
            FindObjectOfType<Canvas>().gameObject.GetComponent<PlayLevel>().enableButtons();
            FindObjectOfType<Canvas>().gameObject.GetComponent<PlayLevel>().select();
            FindObjectOfType<Canvas>().gameObject.GetComponent<PlayLevel>().linesActive(true);
            Debug.Log("No more screens");
            Destroy(gameObject);
        }
    }
}
