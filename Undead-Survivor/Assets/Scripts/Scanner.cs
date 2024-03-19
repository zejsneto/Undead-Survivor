using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange;            // Range of the scanner
    public LayerMask targetLayer;      // Layer mask for the targets
    public RaycastHit2D[] targets;     // Array to store detected targets
    public Transform nearestTarget;    // Nearest detected target

    private void FixedUpdate()
    {
        // Detect targets within the scan range using CircleCastAll
        // This method casts a circle in 2D space and returns all hits within the circle
        // The hits are filtered by the targetLayer
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);

        // Find the nearest target among the detected targets
        nearestTarget = GetNearest();
    }

    // Method to find the nearest target among the detected targets
    private Transform GetNearest()
    {
        Transform result = null;    // Nearest target
        float diff = 100;           // Initial difference set to a high value

        // Iterate through all detected targets
        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;                 // Position of the scanner
            Vector3 targetPos = target.transform.position;      // Position of the current target
            float curDiff = Vector3.Distance(myPos, targetPos); // Calculate distance between scanner and target

            // If the distance to the current target is less than the previous minimum difference,
            // update the nearest target and minimum difference
            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        return result; // Return the nearest target
    }
}
