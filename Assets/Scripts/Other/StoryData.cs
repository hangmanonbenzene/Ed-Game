[System.Serializable]
public class StoryData
{
    public string SaveName;
    public int Level;

    public StoryData(string saveName, int level)
    {
        SaveName = saveName;
        Level = level;
    }
}
