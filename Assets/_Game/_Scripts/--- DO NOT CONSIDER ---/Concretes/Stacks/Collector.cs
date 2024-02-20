using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    public List<Transform> stacks = new List<Transform>();
    public Transform itemHolderTransform;
    public Transform StackParent;
    public int numOfItemsHolding = 0;
    public AnimationCurve collectCurve;
    public AnimationCurve removeCurve;

    float randomDeadRange = 5.0f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            SpreadStacks();
        }
    }

    public void SpreadStacks()
    {
        for (int i = 0; i < numOfItemsHolding; i++)
        {
            StartCoroutine(DeadStackAnimation(i));
        }

        for (int i = 0; i < StackParent.childCount; i++)
        {
            Destroy(StackParent.GetChild(i).gameObject, 1f);
        }
        numOfItemsHolding = 0;
    }

    public IEnumerator DeadStackAnimation(int i)
    {
        {
            stacks.RemoveAt(stacks.Count - 1);
            float timeLapse = 0f;
            float totalTime = 0.2f;

            Transform itemTransform = StackParent.GetChild(numOfItemsHolding - (i + 1)).transform;
            itemTransform.GetComponent<StackController>()._lastCollectible = null;

            Vector3 startPoint = itemTransform.position;
            Vector3 movePoint = transform.position
             + new Vector3(Random.Range(-randomDeadRange, randomDeadRange), 0, Random.Range(-randomDeadRange, randomDeadRange));

            while (timeLapse <= totalTime)
            {
                itemTransform.position = Vector3.Lerp(startPoint, movePoint + itemTransform.transform.up * 3f * collectCurve.Evaluate(timeLapse / totalTime), timeLapse / totalTime);
                timeLapse += Time.deltaTime;
                yield return null;
            }
            yield return null;
        }
    }

    public IEnumerator AddItemAnimation(Transform _itemToAdd)
    {
        float timeLapse = 0f;
        float totalTime = 0.4f;

        Vector3 startPoint;

        startPoint = _itemToAdd.position;
        _itemToAdd.SetParent(StackParent);

        stacks.Add(_itemToAdd);
        if (stacks.Count - 1 >= 15)
        {
            _itemToAdd.GetComponent<StackController>().stackIndex = 15;
        }
        else
        {
            _itemToAdd.GetComponent<StackController>().stackIndex = stacks.Count - 1;

        }

        if (stacks.Count == 1)
        {
            stacks[0].GetComponent<StackController>()._lastCollectible = itemHolderTransform.gameObject;
        }
        else
        {
            stacks[stacks.Count - 1].GetComponent<StackController>()._lastCollectible = stacks[stacks.Count - 2].gameObject;
        }
        while (timeLapse <= totalTime)
        {
            Vector3 movePoint = new Vector3(
                stacks[stacks.Count - 1].GetComponent<StackController>()._lastCollectible.transform.position.x, itemHolderTransform.localPosition.y +
                .4f * numOfItemsHolding,
                stacks[stacks.Count - 1].GetComponent<StackController>()._lastCollectible.transform.position.z);

            _itemToAdd.position = Vector3.Lerp(startPoint, movePoint + Vector3.up * 3f * collectCurve.Evaluate(timeLapse / totalTime), timeLapse / totalTime);
            timeLapse += Time.deltaTime;
            yield return null;
        }
        _itemToAdd.GetComponent<StackController>().canMove = true;
        _itemToAdd.localPosition = new Vector3(_itemToAdd.transform.localPosition.x, .4f * numOfItemsHolding + itemHolderTransform.localPosition.y, _itemToAdd.transform.localPosition.z);
        _itemToAdd.localRotation = Quaternion.identity;
        numOfItemsHolding++;
    }

    public IEnumerator RemoveItemAnimation(Vector3 _house,  int stackCount)
    {
        Transform itemTransform = StackParent.GetChild(numOfItemsHolding - 1).transform;

        int stackIndex = stackCount - numOfItemsHolding;
        numOfItemsHolding--;
        yield return new WaitForSeconds(stackIndex * 0.05f);
        Destroy(itemTransform.gameObject);     
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BaseStack"))
        {  
            other.GetComponent<StackController>().MoveToPlayer();
        }
    }
}
