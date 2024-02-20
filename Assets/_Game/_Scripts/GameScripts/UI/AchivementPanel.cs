using UnityEngine;
using DG.Tweening;
public class AchivementPanel : BaseGamePanel
{
    public float transitionDuration = .25f;
    Vector3 disabledScale = new Vector3(1f, 0f, 1f);
    private void OnEnable()
    {
        transform.localScale = disabledScale;
        AchivementManager.Instance.InitializeGeneralAchivements();
        transform.DOScale(Vector3.one, transitionDuration).SetEase(Ease.OutBack);
        GameManager.Instance.UpdateGameState(GameState.hold);
    }
    private void OnDisable()
    {
        AchivementManager.Instance.InitializeGeneralAchivements();
        GameManager.Instance.UpdateGameState(GameState.playing);
    }
    public override void DisableThisPanel()
    {
        transform.DOScale(disabledScale, transitionDuration).SetEase(Ease.InBack)
            .OnComplete(() => this.gameObject.SetActive(false));
    }
}
