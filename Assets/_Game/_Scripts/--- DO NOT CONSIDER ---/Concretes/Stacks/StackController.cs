using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackController : MonoBehaviour
{
    public bool canMove = false;
    public GameObject _lastCollectible;
    Collector mCollector;
    public int stackIndex;


    private void Awake()
    {
        mCollector = GameObject.FindGameObjectWithTag("Player").GetComponent<Collector>();
    }

    private void Update()
    {
        MoveStacks();
    }

    void MoveStacks()
    {
        if (_lastCollectible != null && canMove)
        {
            // sağa sola kıvrılma için
            transform.position = Vector3.Lerp(transform.position, new Vector3(_lastCollectible.transform.position.x, transform.position.y, _lastCollectible.transform.position.z),
0.80f - (stackIndex * stackIndex * 0.001f));

            // iki stack aras?ndaki z mesafesi için
            //transform.position = new Vector3(transform.position.x, transform.position.y,
            //    _lastCollectible.transform.position.z + _distanceBetweenCollectibles); 
        }
    }

    public void MoveToPlayer()
    {
        if (!canMove)
        {
            canMove = true;
            StartCoroutine(mCollector.AddItemAnimation(this.transform));
        }
    }
}
