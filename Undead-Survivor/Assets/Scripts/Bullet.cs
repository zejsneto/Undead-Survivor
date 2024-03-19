using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Damage inflicted by the bullet
    public float damage;

    // Remaining penetration count
    public int per;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Initialize the bullet with damage, penetration count, and direction
    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage; // Set the damage
        this.per = per; // Set the penetration count

        // If the penetration count is non-negative, set the velocity of the bullet
        if (per >= 0)
        {
            rb.velocity = dir * 15f; // Set the velocity in the specified direction
        }
    }

    // Handle collision with enemy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collision is with an enemy and the bullet can penetrate
        if (collision.CompareTag("Enemy") && per != -100)
        {
            per--; // Decrease the penetration count

            // If the penetration count becomes negative, deactivate the bullet
            if (per < 0)
            {
                rb.velocity = Vector2.zero; // Set velocity to zero
                gameObject.SetActive(false); // Deactivate the bullet
            }
        }
    }

    // Handle collision exit with area (used for bullets that have infinite penetration)
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the collision is with the area and the bullet can penetrate
        if (collision.CompareTag("Area") && per != -100)
        {
            gameObject.SetActive(false); // Deactivate the bullet
        }
    }
}
