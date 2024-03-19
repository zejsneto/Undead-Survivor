using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("# Game Control")]
    public bool isAlive; // Flag indicating whether the player is alive
    public float gameTime; // Current game time
    public float maxGameTime = 2 * 10f; // Maximum game time allowed

    [Header("# Player Info")]
    public int playerId; // ID of the selected player
    public float health; // Current health of the player
    public float maxHealth = 100; // Maximum health of the player
    public int level; // Current level of the player
    public int kill; // Number of enemies killed
    public int exp; // Experience points earned
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 }; // Experience required for each level

    [Header("# Game Object")]
    public PoolManager pool; // Reference to the pool manager for object pooling
    public Player player; // Reference to the player object
    public LevelUp uiLevelUp; // Reference to the level up UI
    public Result uiResult; // Reference to the result UI (win/lose)
    public GameObject enemyCleaner; // Reference to the object responsible for cleaning up enemies

    private void Awake()
    {
        instance = this;
    }

    // Start the game with the specified player ID
    public void GameStart(int id)
    {
        playerId = id;
        health = maxHealth;

        player.gameObject.SetActive(true); // Activate the player object
        uiLevelUp.Select(playerId % 2); // Select the appropriate UI for the player
        Resume(); // Resume the game

        AudioManager.instance.PlayBgm(true); // Play background music
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select); // Play select sound effect
    }

    // Handle game over scenario
    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        isAlive = false;

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Lose(); // Display lose UI
        Stop(); // Stop the game

        AudioManager.instance.PlayBgm(false); // Stop background music
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose); // Play lose sound effect
    }

    // Handle game victory scenario
    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    private IEnumerator GameVictoryRoutine()
    {
        isAlive = false;
        enemyCleaner.SetActive(true); // Activate enemy cleaner object

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win(); // Display win UI
        Stop(); // Stop the game

        AudioManager.instance.PlayBgm(false); // Stop background music
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win); // Play win sound effect
    }

    // Restart the game
    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isAlive)
            return;

        // Check for game over condition (time limit exceeded)
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(0); // Return to main menu if Escape key is pressed

        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameVictory(); // Trigger victory condition if time limit is exceeded
        }
    }

    // Gain experience points
    public void GetExp()
    {
        if (!isAlive)
            return;

        exp++; // Increment experience points

        // Level up if enough experience points are accumulated
        if (exp == nextExp[Mathf.Min(level, nextExp.Length - 1)])
        {
            level++; // Increase level
            exp = 0; // Reset experience points
            uiLevelUp.Show(); // Show level up UI
        }
    }

    // Stop the game
    public void Stop()
    {
        isAlive = false;
        Time.timeScale = 0; // Pause the game
    }

    // Resume the game
    public void Resume()
    {
        isAlive = true;
        Time.timeScale = 1; // Resume normal time scale
    }
}
