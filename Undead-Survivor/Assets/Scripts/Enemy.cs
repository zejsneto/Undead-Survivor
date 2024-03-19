using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Speed of the enemy
    public float speed;

    // Health of the enemy
    public float health;

    // Maximum health of the enemy
    public float maxHealth;

    // Animator controllers for different animations
    public RuntimeAnimatorController[] animCont;

    // Reference to the player's rigidbody
    public Rigidbody2D target;

    // Flag to determine if the enemy is alive
    private bool isAlive;

    private Rigidbody2D rb;
    private Collider2D coll;
    private Animator anim;
    private SpriteRenderer spriteR;
    private WaitForFixedUpdate wait;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriteR = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    private void FixedUpdate()
    {
        // Check if the game is still running
        if (!GameManager.instance.isAlive)
            return;

        // Check if the enemy isn't alive or being hit
        if (!isAlive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        // Enemy movement logic
        Vector2 dirVec = target.position - rb.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + nextVec);
        rb.velocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        // Check if the enemy isn't alive
        if (!isAlive)
            return;

        // Flip enemy sprite direction based on player's position
        spriteR.flipX = target.position.x < rb.position.x;
    }

    // Set the target rigidbody when the enemy is enabled
    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isAlive = true;
        coll.enabled = true;
        rb.simulated = true;
        spriteR.sortingOrder = 2;
        anim.SetBool("Run", true);
        health = maxHealth;
    }

    // Initialize the enemy with spawn data
    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCont[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    // Handle collision with bullets
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collision is with a bullet and if the enemy is alive
        if (!collision.CompareTag("Bullet") || !isAlive)
            return;

        // Decrease enemy's health by the bullet's damage
        health -= collision.GetComponent<Bullet>().damage;

        // Start knockback coroutine
        StartCoroutine(KnockBack());

        // If the enemy is still alive
        if (health > 0)
        {
            anim.SetTrigger("Hit"); // Trigger hit animation
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit); // Play hit sound
        }
        // If the enemy is dead
        else
        {
            isAlive = false; // Mark enemy as dead
            coll.enabled = false; // Disable collider
            rb.simulated = false; // Disable rigidbody simulation
            spriteR.sortingOrder = 1; // Lower sorting order for sprite
            anim.SetBool("Dead", true); // Trigger death animation
            GameManager.instance.kill++; // Increment kill count
            GameManager.instance.GetExp(); // Reward player with experience

            // If the game is still running, play death sound
            // Prevent repetitve death sound when EnemyCleaner is set to active
            if (GameManager.instance.isAlive)
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
        }
    }

    // Coroutine for knockback effect
    private IEnumerator KnockBack()
    {
        yield return wait;
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        // Apply knockback force
        rb.AddForce(dirVec.normalized * 1.2f, ForceMode2D.Impulse);
    }

    // Method called when the enemy is dead
    private void Dead()
    {
        gameObject.SetActive(false);
    }
}
