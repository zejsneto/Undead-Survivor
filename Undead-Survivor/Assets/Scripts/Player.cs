using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;                // Input vector for movement
    public float speed;                     // Movement speed
    public Scanner scanner;                 // Reference to the scanner component
    public Hand[] hands;                    // Array of hand objects
    public RuntimeAnimatorController[] animCon; // Array of animator controllers for different player animations

    private Rigidbody2D rb;                 // Rigidbody component for physics
    private SpriteRenderer spriteR;         // SpriteRenderer component for sprite rendering
    private Animator anim;                  // Animator component for animations

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteR = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        scanner = GetComponent<Scanner>();

        hands = GetComponentsInChildren<Hand>(true);
    }

    private void OnEnable()
    {
        // Adjust player speed based on character stats
        speed *= Character.Speed;

        // Set the animator controller based on the player's ID
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    }

    private void Update()
    {
        // If the game is not alive, do nothing
        if (!GameManager.instance.isAlive)
            return;

        /*
         * Old Input method (New Input done with Unity System)
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
        */
    }

    private void FixedUpdate()
    {
        // If the game is not alive, do nothing
        if (!GameManager.instance.isAlive)
            return;

        // Calculate the next movement vector based on input vector, speed, and fixed delta time
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;

        // Move the player's rigidbody to the next position
        rb.MovePosition(rb.position + nextVec);
    }

    // Input System callback method for movement input
    private void OnMove(InputValue value)
    {
        // Get the input vector from the input value
        inputVec = value.Get<Vector2>();
    }

    private void LateUpdate()
    {
        // If the game is not alive, do nothing
        if (!GameManager.instance.isAlive)
            return;

        // Set animation parameter "Speed" based on the magnitude of input vector
        anim.SetFloat("Speed", inputVec.magnitude);

        // Check if the player is moving horizontally
        if (inputVec.x != 0)
        {
            // Flip player sprite if moving left
            spriteR.flipX = inputVec.x < 0;

            // Set animation parameter "Speed" based on the magnitude of input vector
            anim.SetFloat("Speed", inputVec.magnitude);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // If the game is not alive, do nothing
        if (!GameManager.instance.isAlive)
            return;

        // Decrease player health over time when colliding with objects
        GameManager.instance.health -= Time.deltaTime * 10;

        // Check for death condition
        if (GameManager.instance.health < 0)
        {
            // Deactivate all child game objects except for the first two (Shadow and Area)
            for (int index = 2; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }

            // Trigger death animation
            anim.SetTrigger("Dead");

            // End the game
            GameManager.instance.GameOver();
        }
    }
}
