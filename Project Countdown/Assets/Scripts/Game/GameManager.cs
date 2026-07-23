using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Playing,
        GameOver,
        Won
    }

    // Replace with Game.Manager in all references
    public static GameManager Instance { get; private set; }
    public GameState CurrentState { get; private set; }
    public bool IsGameActive => CurrentState == GameState.Playing;

    public float Oxygen = 100f;

    private void Awake()
    {
        Game.Manager = this;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        CurrentState = GameState.Playing;
        Time.timeScale = 1f;
        Debug.Log("Game started!");
    }

    public void TriggerGameOver() // TODO: Change with polishes like game over screen.
    {
        if (!IsGameActive)
        {
            return;
        }

        CurrentState = GameState.GameOver;
        Debug.Log("Game Over: The player transformed.");
    }

    public void TriggerWin()
    {
        if (!IsGameActive)
        {
            return;
        }

        CurrentState = GameState.Won;

        Debug.Log("The player overloaded the mech core.");
    }
}
