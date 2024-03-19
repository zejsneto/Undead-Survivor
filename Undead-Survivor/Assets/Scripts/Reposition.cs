using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    private Collider2D coll;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    // Called when another Collider2D exits the trigger attached to this object
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Exit the method if collider is not "Area"
        if (!collision.CompareTag("Area"))
            return;

        // Get positions of the player and this object
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;

        // Reposition based on the tag of this object
        switch (transform.tag)
        {
            case "Ground":
                // Calculate the differences in positions
                float diffX = playerPos.x - myPos.x;
                float diffY = playerPos.y - myPos.y;

                // Determine the direction of movement
                float dirX = diffX < 0 ? -1 : 1;
                float dirY = diffY < 0 ? -1 : 1;

                // Take absolute differences
                diffX = Mathf.Abs(diffX);
                diffY = Mathf.Abs(diffY);

                // If the player is further away in the horizontal direction, move the ground horizontally
                if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 40);
                }
                // If the player is further away in the vertical direction, move the ground vertically
                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;
            case "Enemy":
                // If the collider is enabled, move the enemy
                if (coll.enabled)
                {
                    // Calculate distance between player and enemy
                    Vector3 dist = playerPos - myPos;
                    // Add random displacement to enemy position
                    Vector3 rand = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                    // Move the enemy
                    transform.Translate(rand + dist * 2);
                }
                break;
        }
    }
}
