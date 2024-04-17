using UnityEngine;
using TMPro;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public TMP_InputField InputField;
        public TextMeshProUGUI LevelText;
        public GameObject SaveIcon;

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
        }

        public void OnLevelUpPressed()
        {
            SetView();
        }

        public void TryLoadData()
        {
            SaveManager.LoadData<PlayerData>((data) =>
            {

                playerData = data;
                //if load successful remove the input text registertion
                if (playerData == null)
                {
                    playerData = new PlayerData(0, " ");
                }
            });

            SetView();
        }

        private void SetView()
        {
            InputField.text = playerData.Name;
            LevelText.text = $"Level: {playerData.Level}";
        }
    }
}
