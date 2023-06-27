using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimePerTick : MonoBehaviour
{
    [SerializeField] private GameObject play;
    [SerializeField] private GameObject showValue;
    [SerializeField] private GameObject timeOnScreen;

    private float time = 0.5f;

    private bool plus = true;
    private bool minus = true;
    private float resetTime;

    void Start()
    {
        // Set the time to the PlayerPrefs value if it exists
        if (PlayerPrefs.HasKey("TimePerTick"))
        {
            time = PlayerPrefs.GetFloat("TimePerTick");
        }
        else
        {
            PlayerPrefs.SetFloat("TimePerTick", time);
        }
        play.GetComponent<Play>().setTime(time);
        valueChangeTo(time);
    }

    public void onClickPlus()
    {
        time += 0.1f;
        if (time > 1.0f)
            time = 1.0f;
        PlayerPrefs.SetFloat("TimePerTick", time);
        play.GetComponent<Play>().setTime(time);
        valueChangeTo(time);
    }
    public void onClickMinus()
    {
        time -= 0.1f;
        if (time < 0.1f)
            time = 0.1f;
        PlayerPrefs.SetFloat("TimePerTick", time);
        play.GetComponent<Play>().setTime(time);
        valueChangeTo(time);
    }
    public void disableTimeOnScreen()
    {
        timeOnScreen.SetActive(false);
    }

    private void valueChangeTo(float value)
    {
        showValue.GetComponent<TextMeshProUGUI>().text = value.ToString("0.0");
    }

    private void Update()
    {
        resetTime = Mathf.Clamp(resetTime - Time.deltaTime, 0.0f, 1.0f);
        if (Input.GetAxis("test") > 0.5f && plus)
        {
            plus = false;
            onClickPlus();
            if (!GetComponent<PauseLevel>().getPauseMenuActive())
            {
                timeOnScreen.GetComponentInChildren<TextMeshProUGUI>().text = time.ToString("0.0");
                timeOnScreen.SetActive(true);
            }
            resetTime = 0.25f;
        }
        if (Input.GetAxis("test") < -0.5f && minus)
        {
            minus = false;
            onClickMinus();
            if (!GetComponent<PauseLevel>().getPauseMenuActive())
            {
                timeOnScreen.GetComponentInChildren<TextMeshProUGUI>().text = time.ToString("0.0");
                timeOnScreen.SetActive(true);
            }
            resetTime = 0.25f;
        }
        if (!plus)
        {
            if (resetTime <= 0.0f)
            {
                plus = true;
                resetTime = 0.25f;
            }
        }
        if (!minus)
        {
            if (resetTime <= 0.0f)
            {
                minus = true;
                resetTime = 0.25f;
            }
        }
        if (Input.GetAxis("test") < 0.5f && Input.GetAxis("test") > -0.5f)
        {
            plus = true;
            minus = true;
        }
        if (resetTime <= 0.0f)
        {
            timeOnScreen.SetActive(false);
        }
    }
}
