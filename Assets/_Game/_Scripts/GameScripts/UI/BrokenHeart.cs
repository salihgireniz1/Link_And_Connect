using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenHeart : MonoBehaviour
{
    private void OnEnable()
    {
        //GameManager.Instance.UpdateGameState(GameState.hold);
    }
    public void FinishProcess()
    {
        GameManager.Instance.UpdateGameState(GameState.playing);
        Destroy(transform.parent.gameObject);
    }
}
