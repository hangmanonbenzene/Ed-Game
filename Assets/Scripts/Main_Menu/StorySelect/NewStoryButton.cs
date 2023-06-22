using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewStoryButton : SelectButton
{
    public void setupButton(StoryMode storyMode)
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { storyMode.newGame(); });
    }
}
