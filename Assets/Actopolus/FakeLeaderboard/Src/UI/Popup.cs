using System;
using Actopolus.FakeLeaderboard.Src.Tween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Actopolus.FakeLeaderboard.Src.UI
{
    // Leaderboard popup
    public class Popup : MonoBehaviour
    {
        // Leaderboard settings
        [SerializeField] private LeaderBoardData data;

        // Title of popup
        [SerializeField] private TMP_Text title;

        // Player counter animator
        [SerializeField] private Counter playerRankCounter;

        // Player item in front of leaderboard
        [SerializeField] private Item playerItem;

        // Player item container
        [SerializeField] private GameObject playerItemContainer;

        // All leaders
        [SerializeField] private Item[] allLeaders;

        // Popup transform
        [SerializeField] private Transform popup;

        // Scroll view
        [SerializeField] private ScrollRect scrollView;

        // Content rect transform
        [SerializeField] private RectTransform contentRect;

        // Resets popup state
        public void Reset()
        {
            gameObject.SetActive(true);
            popup.localScale = Vector3.zero;
            scrollView.horizontalNormalizedPosition = 0.5f;
            scrollView.enabled = false;
            contentRect.anchoredPosition = Vector2.zero;
        }

        // Shows up popup
        public void Show(int oldRankPosition, int newRankPosition, Action onComplete = null)
        {
            var isUp = newRankPosition <= oldRankPosition;
            var rankChangeSprite = isUp ? data.RankUpSprite : data.RankDownSprite;
            var rankChangeColor = isUp ? data.RankUpColor : data.RankDownColor;
            
            title.text = data.PopupTitle;

            var playerInfo = new PlayerInfo()
            {
                Username = data.PlayerName,
                Country = rankChangeSprite
            };
            
            var index = newRankPosition < 3 ? newRankPosition - 1 : 2;
            if (!isUp)
            {
                index = allLeaders.Length - 3;
            }
            
            var place = newRankPosition - index;

            foreach (var leader in allLeaders)
            {
                leader.Initialize(Leaderboard.Instance.CreatePlayerInfo(), place);
                place++;
            }

            var targetPlayerItem = allLeaders[index];

            playerItemContainer.SetActive(true);
            playerItem.Initialize(playerInfo, oldRankPosition);
            playerItem.SetIconColor(rankChangeColor);
            
            targetPlayerItem.Initialize(playerInfo, newRankPosition);
            targetPlayerItem.SetIconColor(rankChangeColor);
            targetPlayerItem.HideContent();

            playerRankCounter.SetDuration(data.RankCounterAnimationDuration);

            if (data.PopupShowAnimationDuration <= 0f)
            {
                popup.localScale = Vector3.one;
                playerRankCounter.SetCount(newRankPosition);
                scrollView.enabled = true;

                ScrollRank(isUp, () => {
                    playerItemContainer.SetActive(false);
                    targetPlayerItem.ShowContent();
                    onComplete?.Invoke();
                });
                return;
            }

            popup.localScale = Vector3.zero;
            scrollView.enabled = false;

            ResizePopup(popup.localScale, Vector3.one, data.PopupShowAnimationDuration, () => {
                scrollView.enabled = true;
                playerRankCounter.SetCount(newRankPosition);

                ScrollRank(isUp, () => {
                    playerItemContainer.SetActive(false);
                    targetPlayerItem.ShowContent();
                    onComplete?.Invoke();
                });
            });
        }

        // Hides popup
        public void Hide(Action onComplete = null)
        {
            if (!gameObject.activeSelf)
            {
                return;
            }
            
            if (data.PopupHideAnimationDuration <= 0f)
            {
                gameObject.SetActive(false);
                onComplete?.Invoke();
                return;
            }

            ResizePopup(popup.localScale, Vector3.zero, data.PopupHideAnimationDuration, () => {
                gameObject.SetActive(false);
                onComplete?.Invoke();
            });
        }

        // Animates rank scrolling
        private void ScrollRank(bool isUp, Action onComplete)
        {
            var scrollTo = isUp ? 1f : 0f;

            TwAnimation.Value(this, 0.5f, scrollTo, v => {
                scrollView.verticalNormalizedPosition = v;
            }, data.RankCounterAnimationDuration, 0f, onComplete);
        }

        // Animates popup scale
        private void ResizePopup(Vector3 original, Vector3 target, float duration, Action onComplete)
        {
            TwAnimation.Value(this, 0f, 1f, v => { popup.localScale = Vector3.Lerp(original, target, v); },
                duration, 0f, onComplete);
        }
    }
}