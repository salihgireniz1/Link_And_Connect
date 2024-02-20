using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonAnimation
{
    private Button _button;
    private Sequence _sequence;
    private Vector3 _buttonInitialScale = Vector3.zero;

    public ButtonAnimation(Button button)
    {
        _button = button;
        _buttonInitialScale = _button.transform.localScale;
        SetButtonNonInteractive();
        SetButtonDisable();
        SetButtonScaleZero();
    }

    public void ShowButtonWithGrowingAnimation(float duration, Ease ease)
    {
        SetButtonEnable();
        SetButtonScaleZero();
        _button.transform.DOScale(_buttonInitialScale, duration).SetEase(ease).OnComplete(() => OnButtonAnimationCompleted());
    }

    private void OnButtonAnimationCompleted()
    {
        _button.transform.localScale = _buttonInitialScale;
        SetButtonInteractive();
        PlayButtonIdleAnimation(0.5f);
    }

    private void PlayButtonIdleAnimation(float duration)
    {
        _sequence = DOTween.Sequence();
        Vector3 endScale = _buttonInitialScale + new Vector3(0.1f, 0.1f, 0.1f);

        _sequence.Append(_button.transform.DOScale(endScale, duration)).SetEase(Ease.Linear);
        _sequence.Append(_button.transform.DOScale(_buttonInitialScale, duration)).SetEase(Ease.Linear).SetLoops(-1);
    }


    private void SetButtonNonInteractive()
    {
        _button.interactable = false;
    }

    private void SetButtonInteractive()
    {
        _button.interactable = true;
    }

    private void SetButtonEnable()
    {
        _button.gameObject.SetActive(true);
    }

    private void SetButtonDisable()
    {
        _button.gameObject.SetActive(false);
    }

    private void SetButtonScaleZero()
    {
        _button.transform.localScale = Vector3.zero;
    }
    public void Reset()
    {
        SetButtonNonInteractive();
        SetButtonScaleZero();
        SetButtonDisable();
        _sequence.Kill();
    }
}
