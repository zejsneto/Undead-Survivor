using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;   // Array of spawn points
    public SpawnData[] spawnData;    // Array of spawn data
    public float levelTime;          // Time interval for each level

    private int level;      // Current level
    private float timer;    // Timer for spawning enemies

    private void Awake()
    {
        // Get spawn points
        spawnPoint = GetComponentsInChildren<Transform>();

        // Calculate level time 
        levelTime = GameManager.instance.maxGameTime / spawnData.Length;
    }

    private void Update()
    {
        // If the player is not alive, do nothing
        if (!GameManager.instance.isAlive)
            return;

        timer += Time.deltaTime;

        // Determine current level based on game time and level time
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime), spawnData.Length - 1);

        // If it's time to spawn enemies for the current level, spawn them
        if (timer > spawnData[level].spawnTime)
        {
            timer = 0;
            Spawn();
        }
    }

    // Spawn enemies
    private void Spawn()
    {
        // Get an enemy object from the object pool
        GameObject enemy = GameManager.instance.pool.Get(0);

        // Choose a random spawn point for the enemy
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;

        // Initialize the enemy with spawn data for the current level
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}


[System.Serializable]
public class SpawnData
{
    public float spawnTime;     // Time delay for spawning enemies
    public int spriteType;      // Type of sprite for the enemy
    public int health;          // Health of the enemy
    public float speed;         // Speed of the enemy
}
