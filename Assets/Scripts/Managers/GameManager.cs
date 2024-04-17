using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TMP_InputField InputField;
    public TextMeshProUGUI LevelText;
    public GameObject saveIcon;
    private int counter;

    private PlayerData playerData;

    private void Start()
    {
        //SaveManager.LoadData<PlayerData>(SetView);    
    }

    public void OnDonePressed()
    {
        saveIcon.SetActive(true);

        SaveManager.SaveData(new PlayerData(counter, InputField.text));

        saveIcon.SetActive(false);
    }

    public void OnLevelUpPressed()
    {
         SaveManager.SaveData(new PlayerData(counter, InputField.text));
    }

    public void TestLoad()
    {
        playerData = new PlayerData(0," ");

        SaveManager.LoadData<PlayerData>(SetView);
    }

    private void SetView(PlayerData data)
    {
        playerData = data;

        InputField.text = playerData.Name;
        LevelText.text = $"Level: {data.Level}";
    }
}
