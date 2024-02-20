using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Actopolus.FakeLeaderboard.Src.UI
{
    // Item presentation
    public class Item : MonoBehaviour
    {
        // Item content container
        [SerializeField] private GameObject content;
        
        // Country icon
        [SerializeField] private Image icon;
        
        // Username text
        [SerializeField] private TMP_Text username;

        // Place counter
        [SerializeField] private Counter placeCounter;

        // Initializes item
        public void Initialize(PlayerInfo playerInfo, int rank)
        {
            SetPlayerInfo(playerInfo);
            SetRank(rank);
        }

        // Sets color for country icon
        public void SetIconColor(Color color) => icon.color = color;

        // Hides content of item
        public void HideContent() => content.SetActive(false);

        // Shows content of item
        public void ShowContent() => content.SetActive(true);

        // Sets player info
        private void SetPlayerInfo(PlayerInfo playerInfo)
        {
            icon.sprite = playerInfo.Country;
            username.text = playerInfo.Username;
        }

        // Sets rank position
        private void SetRank(int rank) => placeCounter.SetCountQuiet(rank);
    }
}