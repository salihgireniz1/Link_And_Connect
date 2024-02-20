using System;
using System.Collections;
using Actopolus.FakeLeaderboard.Src.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Actopolus.FakeLeaderboard.Src
{
    // Leaderboard manager
    public class Leaderboard : MonoBehaviour
    {
        // Player's rank key in player prefs
        private const string PlayerRankKey = "actopolus.player.rank";
        
        // Data generator for leaderboard
        [SerializeField] private LeaderBoardData data;

        // Leaderboard popup prefab
        [SerializeField] private Popup leaderBoardPopup;

        // Default event system prefab
        [SerializeField] private EventSystem eventSystem;
        
        // Leaderboard instance
        private static Leaderboard _instance;

        // Returns instance of leader board
        public static Leaderboard Instance => _instance;

        // Popup
        private Popup _popup;

        // Initializes singleton
        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
        }

        // Returns random player info
        public PlayerInfo CreatePlayerInfo() => data.CreatePlayerInfo();

        // Returns current rank
        public int GetRank() => PlayerPrefs.GetInt(PlayerRankKey, Random.Range(
            data.MinInitialRank,
            data.MaxInitialRank
        ));

        // Resets player rank
        public void Reset()
        {
            PlayerPrefs.SetInt(PlayerRankKey, Random.Range(
                data.MinInitialRank,
                data.MaxInitialRank
            ));
            PlayerPrefs.Save();
        }

        // Shows rank popup with auto progress
        public void Show(Action onComplete = null)
        {
            var oldRank = GetRank();
            var newRank = oldRank - Random.Range(data.MinRankStep, data.MaxRankStep);
            newRank = Mathf.Max(1, newRank);

            Show(oldRank, newRank, onComplete);
        }

        // Shows rank popup without auto progress
        public void Show(int oldRankPosition, int newRankPosition, Action onComplete = null)
        {
            PlayerPrefs.SetInt(PlayerRankKey, newRankPosition);
            PlayerPrefs.Save();

            StartCoroutine(ShowCoroutine(oldRankPosition, newRankPosition, onComplete));
        }

        // Hides rank popup
        public void Hide(Action onComplete = null)
        {
            if (_popup == null)
            {
                return;
            }

            _popup.Hide(onComplete);
        }

        // Shows popup
        private IEnumerator ShowCoroutine(int oldRankPosition, int newRankPosition, Action onComplete = null)
        {
            InitializeComponents();

            _popup.Reset();
            yield return null;
            _popup.Show(oldRankPosition, newRankPosition, onComplete);
        }

        // Initializes canvas and input event system
        private void InitializeComponents()
        {
            var es = FindObjectOfType<EventSystem>(true);
            _popup = FindObjectOfType<Popup>(true);

            if (_popup == null)
            {
                _popup = Instantiate(leaderBoardPopup);
            }

            if (es == null)
            {
                Instantiate(eventSystem);
            }
        }
    }
}