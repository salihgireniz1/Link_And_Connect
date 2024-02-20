namespace Actopolus.FakeLeaderboard.Src.Tween
{
    // Easing methods
    public class Easing
    {
        // Easing: Ease in, Ease out
        public static float LinearEaseInOut(float elapsedTime, float min, float max, float duration)
        {
            return max * elapsedTime / duration + min;
        }
    }
}