using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // Array of prefabs
    public GameObject[] prefabs;

    // List to hold pooled objects
    List<GameObject>[] pools;

    private void Awake()
    {
        // Pools array with the same length as the prefabs array
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; ++index)
        {
            // Each element of the pools array is a new List<GameObject>
            pools[index] = new List<GameObject>();
        }
    }

    // Method to get an object from the pool by index
    public GameObject Get(int index)
    {
        GameObject select = null;

        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                // Set select to the inactive item
                select = item;
                // Activate the selected item
                select.SetActive(true);
                break;
            }
        }

        if (!select)
        {
            // Instantiate a new object from the corresponding prefab
            select = Instantiate(prefabs[index], transform);
            // Add the newly instantiated object to the pool
            pools[index].Add(select);
        }

        return select;
    }
}