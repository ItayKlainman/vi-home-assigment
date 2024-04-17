using UnityEngine;
using TMPro;
using Managers;
using System;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private TMP_InputField InputField;

        [SerializeField] private TextMeshProUGUI LevelText;
        [SerializeField] private TextMeshProUGUI playerName;

        [SerializeField] private GameObject registertionPanel;
        [SerializeField] private GameObject gameStartPanel;

        private PlayerData playerData;
        private bool isNewSave;

        private const string DATA_KEY = "player_data";


        private void Start()
        {
           TryLoadData();
        }
        
        public void StartGame()
        {
            TryLoadData();
            gameStartPanel.SetActive(false);

            UpdateDataAndView(playerData.Level, playerData.Name);
        }

        public void OnRegisterPressed()
        {
            playerData = new PlayerData(1, InputField.text);

            SaveManager.SaveData(playerData, DATA_KEY);

            registertionPanel.SetActive(false);
            SetView();
        }

        public void OnLevelUpPressed()
        {
            UpdateDataAndView(playerData.Level + 1, playerData.Name);
        }

        public void UpdateDataAndView(int playerLevel, string playerName)
        {
            playerData = new PlayerData(playerLevel, playerName);
            SaveManager.SaveData(playerData, DATA_KEY);
            SetView();
        }

        public void TryLoadData()
        {
            SaveManager.LoadData<PlayerData>(DATA_KEY, (data) =>
            {
                if (data == null)
                {
                    playerData = new PlayerData(1, string.Empty);
                    isNewSave = true;
                }
                else
                {
                    playerData = data;
                }

                SetView();
            });
        }

        private void SetView()
        {
            if(isNewSave)
            {
                registertionPanel.SetActive(true);
                isNewSave = false;
            }

            playerName.SetText($"Player: {playerData.Name}");
            LevelText.text = $"Level: {playerData.Level}";
        }
    }
}
