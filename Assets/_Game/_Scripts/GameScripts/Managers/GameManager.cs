using Sirenix.OdinInspector;
using System;
using UnityEngine;

public enum GameState
{
    loaded,
    playing,
    hold,
    failed,
    completed,
}
public class GameManager : MonoSingleton<GameManager>
{
    public static event Action<GameState> OnGameStateChanged;
    public GameState GameState
    {
        get => gameState;
        private set
        {
            gameState = value;
            Debug.Log("Game: " + gameState.ToString());
            OnGameStateChanged?.Invoke(gameState);
        }
    }
    [SerializeField]
    private GameState gameState = GameState.loaded;
    private void OnEnable()
    {
        LevelManager.OnLevelLoaded += InitializeGame;
    }
    private void OnDisable()
    {
        LevelManager.OnLevelLoaded -= InitializeGame;
    }
    public void InitializeGame(Level level)
    {
        UpdateGameState(GameState.loaded);
    }
    [Button]
    public void UpdateGameState(GameState state)
    {
        GameState = state;
    }
}
