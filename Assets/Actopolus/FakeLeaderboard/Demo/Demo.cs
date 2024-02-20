using Actopolus.FakeLeaderboard.Src;
using UnityEngine;

namespace Actopolus.FakeLeaderboard.Demo
{
    // Demo actions
    public class Demo : MonoBehaviour
    {
        // Handles click on "Show" button
        public void OnClickShow() => Leaderboard.Instance.Show(() => {
            Debug.Log("Rank popup animation is finished!");
        });

        // Handles click on "Hide" button
        public void OnClickHide() => Leaderboard.Instance.Hide(() => {
            Debug.Log("Rank is hidden");
        });

        // Handles click on "Reset" button
        public void OnClickReset() => Leaderboard.Instance.Reset();
    }
}