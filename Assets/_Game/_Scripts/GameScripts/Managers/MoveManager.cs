using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoveManager : MonoSingleton<MoveManager>
{
    public static event Action<int> OnMove;
    public bool canCheck;
    public int MoveCount
    {
        get => moveCount;
        set
        {
            canCheck = true;
            moveCount = value;
            OnMove?.Invoke(moveCount);
        }
    }

    [SerializeField]
    private int moveCount;

    private void OnEnable()
    {
        LevelManager.OnLevelLoaded += ResetMoveCount;
        //MergeManager.OnSpritesMerged += HandleMerge;
        GameManager.OnGameStateChanged += CheckGameState;
    }
    private void OnDisable()
    {
        LevelManager.OnLevelLoaded -= ResetMoveCount;
        //MergeManager.OnSpritesMerged -= HandleMerge;
        GameManager.OnGameStateChanged -= CheckGameState;
    }
    public void DecreaseMoveCount()
    {
        MoveCount -= 1;
    }
    public void ResetMoveCount(Level level)
    {
        MoveCount = level.levelMoveCount;
    }
    void HandleMerge(List<SpriteInfo> sprites)
    {
        DecreaseMoveCount();
    }
    void CheckGameState(GameState state)
    {
        if(state == GameState.playing)
        {
            if (moveCount == 0 && canCheck)
            {
                GameManager.Instance.UpdateGameState(GameState.failed);
                canCheck = false;
                Debug.Log("OUT OF MOVES");
            }
        }
    }
}