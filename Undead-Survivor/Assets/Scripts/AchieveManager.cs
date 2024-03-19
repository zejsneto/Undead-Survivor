using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchieveManager : MonoBehaviour
{
    public GameObject[] lockCharacter; // Array of locked character GameObjects
    public GameObject[] unlockCharacter; // Array of unlocked character GameObjects
    public GameObject uiNotice; // UI notification object

    // Enumeration representing different achievements
    enum Achieve { UnlockSteve, UnlockEmily }
    Achieve[] achieves; // Array to hold all achievements
    WaitForSecondsRealtime wait; // WaitForSecondsRealtime object for delays

    void Awake()
    {
        // Initialize the achieves array with all values of the Achieve enum
        achieves = (Achieve[])Enum.GetValues(typeof(Achieve));
        wait = new WaitForSecondsRealtime(5); // Initialize WaitForSecondsRealtime with a delay of 5 seconds

        // If PlayerPrefs doesn't have the key "MyData", initialize PlayerPrefs
        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
    }

    // Initialize PlayerPrefs with default values
    void Init()
    {
        PlayerPrefs.SetInt("MyData", 1); // Set the key "MyData" to 1

        // Initialize each achievement to 0 (not unlocked)
        foreach (Achieve achieve in achieves)
        {
            PlayerPrefs.SetInt(achieve.ToString(), 0);
        }
    }

    void Start()
    {
        UnlockCharacter(); // Call the method to unlock characters
    }

    // Unlock characters based on achievements
    void UnlockCharacter()
    {
        // Loop through each character
        for (int index = 0; index < lockCharacter.Length; index++)
        {
            string achieveName = achieves[index].ToString(); // Get the name of the achievement
            bool isUnlock = PlayerPrefs.GetInt(achieveName) == 1; // Check if the achievement is unlocked
            lockCharacter[index].SetActive(!isUnlock); // Set the locked character GameObject active or inactive based on unlock status
            unlockCharacter[index].SetActive(isUnlock); // Set the unlocked character GameObject active or inactive based on unlock status
        }
    }

    void LateUpdate()
    {
        // Check each achievement
        foreach (Achieve achieve in achieves)
        {
            CheckAchieve(achieve);
        }
    }

    // Check if a specific achievement is fulfilled
    void CheckAchieve(Achieve achieve)
    {
        bool isAchieve = false;

        // Check conditions for each achievement
        switch (achieve)
        {
            case Achieve.UnlockSteve:
                isAchieve = GameManager.instance.kill >= 100; // Check if player has killed 100 enemies
                break;
            case Achieve.UnlockEmily:
                isAchieve = GameManager.instance.gameTime == GameManager.instance.maxGameTime; // Check if game time equals max game time (Player Survived)
                break;
        }

        // If the achievement is fulfilled and not yet unlocked
        if (isAchieve && PlayerPrefs.GetInt(achieve.ToString()) == 0)
        {
            // Set the achievement as unlocked in PlayerPrefs
            PlayerPrefs.SetInt(achieve.ToString(), 1);

            // Activate the corresponding notification in the UI
            for (int index = 0; index < uiNotice.transform.childCount; index++)
            {
                bool isActive = index == (int)achieve;
                uiNotice.transform.GetChild(index).gameObject.SetActive(isActive);
            }

            // Start coroutine to display the notification
            StartCoroutine(NoticeRoutine());
        }
    }

    // Coroutine to display the notification for a certain duration
    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true); // Activate the UI notification
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp); // Play level up sound effect

        yield return wait; // Wait for the specified duration

        uiNotice.SetActive(false); // Deactivate the UI notification
    }
}
