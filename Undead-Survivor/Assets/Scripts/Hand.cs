using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    // Boolean to determine if this hand is for the left side
    public bool isLeft;

    // Reference to the SpriteRenderer component of this hand
    public SpriteRenderer spriteR;

    // Reference to the SpriteRenderer component of the player
    private SpriteRenderer player;

    // Positions and rotations for the hand when it's on the right side
    private Vector3 rightPos = new Vector3(0.35f, -0.15f, 0);
    private Vector3 rightPosReverse = new Vector3(-0.15f, -0.15f, 0);
    private Quaternion leftRot = Quaternion.Euler(0, 0, -35);
    private Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135);

    private void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];
    }

    private void LateUpdate()
    {
        // Check if the player is flipped horizontally
        bool isReverse = player.flipX;

        if (isLeft)
        {
            // Set the local rotation of the hand based on player flip direction
            transform.localRotation = isReverse ? leftRotReverse : leftRot;
            // Flip the hand sprite vertically based on player flip direction
            spriteR.flipY = isReverse;
            // Set the sorting order of the hand sprite based on player flip direction
            spriteR.sortingOrder = isReverse ? 4 : 6;
        }
        else
        {
            // Set the local position of the hand based on player flip direction
            transform.localPosition = isReverse ? rightPosReverse : rightPos;
            // Flip the hand sprite horizontally based on player flip direction
            spriteR.flipX = isReverse;
            // Set the sorting order of the hand sprite based on player flip direction
            spriteR.sortingOrder = isReverse ? 6 : 4;
        }
    }
}
