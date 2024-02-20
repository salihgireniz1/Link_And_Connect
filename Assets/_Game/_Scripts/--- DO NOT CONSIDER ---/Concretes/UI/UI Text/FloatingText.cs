using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class FloatingText
{
    private GameObject _panelGameObject;
    private TextMeshProUGUI _panelText;
    private Camera _camera;
    private RectTransform _panelRectTransform;

    public FloatingText(GameObject panelGameObject)
    {
        _panelGameObject = panelGameObject;
        _panelText = _panelGameObject.GetComponentInChildren<TextMeshProUGUI>();
        _camera = Camera.main;
        _panelRectTransform = _panelGameObject.GetComponent<RectTransform>();
    }

    public IEnumerator StartHorizontalFloatingText(GameObject playerGameObject, Vector3 offSet,float duration ,string panelText)
    {
        _panelGameObject.SetActive(true);
        _panelGameObject.transform.position = _camera.WorldToScreenPoint(playerGameObject.transform.position + offSet);
        _panelText.text = panelText;

        float posX = _panelRectTransform.localPosition.x + 100f;
        _panelRectTransform.DOAnchorPosX(posX, duration * 3f);

        Color textColor = _panelText.color;

        float time = 0;

        while(time <= 1)
        {
            textColor.a = Mathf.Lerp(0, 1, time);
            _panelText.color = textColor;
            time += Time.deltaTime / duration;
            yield return null;
        }

        yield return new WaitForSeconds(0.4f);

        time = 1;
        while (time >= 0)
        {
            textColor.a = Mathf.Lerp(0, 1, time);
            _panelText.color = textColor;
            time -= Time.deltaTime / duration;
            yield return null;
        }

        _panelGameObject.SetActive(false);
    }
}
