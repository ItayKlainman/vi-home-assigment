using UnityEngine;

public class PlayerData : ISavableData
{
    public int Level;
    public string Name;

    public PlayerData(int level, string name)
    {
        Level = level;
        Name = name;
    }
}
