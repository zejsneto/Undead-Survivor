using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    public GameObject[] titles; 

    // Display lose title
    public void Lose()
    {
        titles[0].gameObject.SetActive(true);
    }

    // Display win title
    public void Win()
    {
        titles[1].gameObject.SetActive(true);
    }
}
