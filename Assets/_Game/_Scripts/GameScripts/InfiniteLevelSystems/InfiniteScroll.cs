using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfiniteScroll : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField] ScrollRect _scrollRect;
    [SerializeField] Transform _transform;
    [SerializeField] float _outOfBoundsThreshold = 40f;
    [SerializeField] float _childWidth = 125f;
    [SerializeField] float _childHeight = 125f;
    [SerializeField] float _itemSpacing = 30f;

    Vector2 _lastDragPosition;
    bool _positiveDrag;
    int _childCount = 0;
    float _height = 0f;

    IEnumerator Start()
    {
        _scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
        _childCount = _scrollRect.content.childCount;
        _height = Screen.height;
        _scrollRect.content.localPosition = Vector3.up * _height * 2f;

        yield return new WaitForSeconds(1f);
        int counter = 0;
        while (counter < 300)
        {
            _positiveDrag = true;
            HandleScrollRectValueChanged(Vector2.zero);
            counter++;
            _scrollRect.content.transform.Translate(Time.deltaTime * 3f * Vector2.up);
            yield return null;
        }

        Vector2 pos = _scrollRect.transform.localPosition;
        pos.y = 0;
        _scrollRect.content.transform.localPosition = pos;
    }

    void OnEnable()
    {
        _scrollRect.onValueChanged.AddListener(HandleScrollRectValueChanged);
    }

    void OnDisable()
    {
        _scrollRect.onValueChanged.RemoveListener(HandleScrollRectValueChanged);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _lastDragPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 newPosition = eventData.position;
        _positiveDrag = newPosition.y > _lastDragPosition.y;
        _lastDragPosition = newPosition;
    }

    bool ReachedThreshold(Transform item)
    {
        float positiveYThreshold = _transform.position.y + _height * 0.5f + _outOfBoundsThreshold;
        float negativeYThreshold = _transform.position.y - _height * 0.5f - _outOfBoundsThreshold;
        return _positiveDrag
            ? item.position.y - _childWidth * 0.5f > positiveYThreshold
            : item.position.y + _childWidth * 0.5f < negativeYThreshold;
    }

    void HandleScrollRectValueChanged(Vector2 value)
    {


        int currentItemIndex = _positiveDrag ? _childCount - 1 : 0;
        Transform currentItem = _scrollRect.content.GetChild(currentItemIndex);

        if (!ReachedThreshold(currentItem))
        {

            return;
        }
        int endItemIndex = _positiveDrag ? 0 : _childCount - 1;
        Transform endItem = _scrollRect.content.GetChild(endItemIndex);
        Vector2 newPosition = endItem.position;

        if (_positiveDrag)
        {
            newPosition.y = endItem.position.y - _childHeight * 1.5f - _itemSpacing;
        }
        else
        {
            newPosition.y = endItem.position.y + _childHeight * 1.5f + _itemSpacing;
        }
        currentItem.position = newPosition;
        currentItem.SetSiblingIndex(endItemIndex);
    }
}
