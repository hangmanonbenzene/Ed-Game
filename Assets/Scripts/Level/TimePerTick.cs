using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimePerTick : MonoBehaviour
{
    [SerializeField] private GameObject play;
    [SerializeField] private GameObject showValue;
    private float time = 0.5f;

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

    private void valueChangeTo(float value)
    {
        showValue.GetComponent<TextMeshProUGUI>().text = value.ToString("0.0");
    }
}
