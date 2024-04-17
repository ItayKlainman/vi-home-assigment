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

        private void Start()
        {
            TryLoadData();
        }

        public void OnDonePressed()
        {
            SaveIcon.SetActive(true);

            playerData = new PlayerData(1, InputField.text);

            SaveManager.SaveData(playerData);

            SaveIcon.SetActive(false);

            SetView();
        }

        public void OnLevelUpPressed()
        {
            playerData = new PlayerData(playerData.Level + 1, playerData.Name);
            SaveManager.SaveData(playerData);
            SetView();
        }

        public void TryLoadData()
        {
            SaveManager.LoadData<PlayerData>((data) =>
            {
                playerData = data ?? new PlayerData(0, "");
                SetView();
            });
        }

        private void SetView()
        {
            if(playerData == null)
            {
                Debug.Log("playerdata is null");
            }

            playerName.SetText(playerData.Name);
            LevelText.text = $"Level: {playerData.Level}";
        }
    }
}
