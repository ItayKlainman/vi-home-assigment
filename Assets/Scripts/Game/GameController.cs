using UnityEngine;
using TMPro;
using Managers;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private TMP_InputField InputField;
        [SerializeField] private TextMeshProUGUI LevelText;
        [SerializeField] private TextMeshProUGUI playerName; 
        [SerializeField] private GameObject SaveIcon;

        private PlayerData playerData;
        private const string DATA_KEY = "player_data";

        private void Start()
        {
            TryLoadData();
        }

        public void OnDonePressed()
        {
            SaveIcon.SetActive(true);

            playerData = new PlayerData(1, InputField.text);

            SaveManager.SaveData(playerData, DATA_KEY);

            SaveIcon.SetActive(false);

            SetView();
        }

        public void OnLevelUpPressed()
        {
            playerData = new PlayerData(playerData.Level + 1, playerData.Name);
            SaveManager.SaveData(playerData, DATA_KEY);
            SetView();
        }

        public void TryLoadData()
        {
            SaveManager.LoadData<PlayerData>(DATA_KEY, (data) =>
            {
                playerData = data ?? new PlayerData(0, "");
                SetView();
            });
        }

        private void SetView()
        {
            playerName.SetText($"Player: {playerData.Name}");
            LevelText.text = $"Level: {playerData.Level}";
        }
    }
}
