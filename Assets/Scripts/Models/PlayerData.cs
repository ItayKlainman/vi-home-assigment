using System;

[Serializable]
public class PlayerData
{
    public int Level;
    public string Name;

    public PlayerData(int level, string name)
    {
        Level = level;
        Name = name;
    }
}
