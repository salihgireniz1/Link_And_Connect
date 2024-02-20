using System.Collections.Generic;
using UnityEngine;

namespace Actopolus.FakeLeaderboard.Src
{
    // Player info item
    public struct PlayerInfo
    {
        // Country flag
        public Sprite Country;
        
        // Username
        public string Username;
    }

    // Settings scriptable with usernames and countries 
    [CreateAssetMenu(fileName = "LeaderBoardData", menuName = "Actopolus/FakeLeaderboard/LeaderBoardData", order = 0)]
    public class LeaderBoardData : ScriptableObject
    {
        [Header("Common settings")]
        // Default player name
        [SerializeField] private string playerName;

        // Text asset with usernames
        [SerializeField] private TextAsset usernamesText;
        
        // List of flags
        [SerializeField] private List<Sprite> countries;

        [Header("Popup")] 
        
        // Popup title
        [SerializeField] private string popupTitle;
        
        // Rank up sprite
        [SerializeField] private Sprite rankUpSprite;
        
        // Rank up color
        [SerializeField] private Color rankUpColor;

        // Rank down sprite
        [SerializeField] private Sprite rankDownSprite;

        // Rank down color
        [SerializeField] private Color rankDownColor;

        // Animation duration for rank counter
        [SerializeField] private float rankCounterAnimationDuration;

        // Popup appear animation duration
        [SerializeField] private float popupAppearDuration;

        // Popup disappear animation duration
        [SerializeField] private float popupDisappearDuration;

        [Header("Progress")]

        // Min initial rank position in leaderboard
        [SerializeField] private int minInitialRankPosition;
        
        // Max initial rank position in leaderboard
        [SerializeField] private int maxInitialRankPosition;

        // Minimum rank position decrement for one step
        [SerializeField] private int minRankDecrement;

        // Maximum rank position decrement for one step
        [SerializeField] private int maxRankDecrement;

        // Parsed usernames
        private string[] _usernames;
        
        // Returns default player name
        public string PlayerName => playerName;

        // Popup title
        public string PopupTitle => popupTitle;

        // Returns rank up sprite
        public Sprite RankUpSprite => rankUpSprite;

        // Returns rank up color
        public Color RankUpColor => rankUpColor;
        
        // Returns rank down color
        public Color RankDownColor => rankDownColor;
        
        // Returns rank down sprite
        public Sprite RankDownSprite => rankDownSprite;

        // Returns min initial rank for auto progress
        public int MinInitialRank => minInitialRankPosition;

        // Returns max initial rank for auto progress
        public int MaxInitialRank => maxInitialRankPosition;

        // Returns min rank step in auto progress
        public int MinRankStep => minRankDecrement;
        
        // Returns max rank step in auto progress
        public int MaxRankStep => maxRankDecrement;

        // Returns counter animation duration
        public float RankCounterAnimationDuration => rankCounterAnimationDuration;
        
        // Returns show animation duration
        public float PopupShowAnimationDuration => popupAppearDuration;

        // Returns hide animation duration
        public float PopupHideAnimationDuration => popupDisappearDuration;
        
        // Creates player info with random name and country flag
        public PlayerInfo CreatePlayerInfo()
        {
            if (_usernames == null || _usernames.Length == 0)
            {
                InitializeUsernames();
            }

            return new PlayerInfo()
            {
                Username = _usernames[Random.Range(0, _usernames.Length)],
                Country = countries[Random.Range(0, countries.Count)]
            };
        }

        // Initializes usernames from text file
        private void InitializeUsernames() => _usernames = usernamesText.text.Split('\n');
    }
}