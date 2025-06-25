using UnityEngine;
using System;

public enum GameState
{
    Menu,
    Preparation,
    Fly,
    Workshop,
    RoverDriving
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState CurrentState { get; private set; }

    public event Action<GameState> OnStateChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        ChangeState(GameState.Menu); // Default start state
        Debug.Log("Current state:" + CurrentState);
    }

    public void ChangeState(GameState newState)
    {
        if (newState == CurrentState) return;

        ExitState(CurrentState);
        CurrentState = newState;
        EnterState(CurrentState);

        Debug.Log($"GameManager: Changed state to {CurrentState}");
        OnStateChanged?.Invoke(CurrentState);
    }

    private void EnterState(GameState state)
    {
        switch (state)
        {
            case GameState.Menu:
                // Show menu UI
                break;
            case GameState.Preparation:
                // Prep launch
                break;
            case GameState.Fly:
                // Start rocket
                break;
            case GameState.Workshop:
                // Workshop logic
                break;
            case GameState.RoverDriving:
                // Rover mode
                break;
        }
    }

    private void ExitState(GameState state)
    {
        // You can customize this too
    }
}
