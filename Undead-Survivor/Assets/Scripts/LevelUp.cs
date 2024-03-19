using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    private RectTransform rect;

    // Array to store references to all Item components attached to children of this object
    private Item[] items;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();

        items = GetComponentsInChildren<Item>(true);
    }

    // Method to show the level up menu
    public void Show()
    {
        // Display the next set of items
        Next();

        // Set the local scale of the RectTransform to make it visible
        rect.localScale = Vector3.one;

        // Pause the game
        GameManager.instance.Stop();

        // Play level up sound effect
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);

        // Enable level up effect on background music
        AudioManager.instance.EffectBgm(true);
    }

    // Method to hide the level up menu
    public void Hide()
    {
        // Hide the level up menu
        rect.localScale = Vector3.zero;

        // Resume the game
        GameManager.instance.Resume();

        // Play select sound effect
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);

        // Disable level up effect on background music
        AudioManager.instance.EffectBgm(false);
    }

    // Method to select an item
    public void Select(int index)
    {
        // OnClick method for the selected item
        items[index].OnClick();
    }

    // Method to prepare the next set of items
    private void Next()
    {
        // 1. Disable all items
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        // 2. Activate 3 distinct random items among them
        int[] rand = new int[3];
        while (true)
        {
            rand[0] = Random.Range(0, items.Length);
            rand[1] = Random.Range(0, items.Length);
            rand[2] = Random.Range(0, items.Length);

            // Ensure that the selected items are distinct
            if (rand[0] != rand[1] && rand[1] != rand[2] && rand[0] != rand[2])
                break;
        }

        // Loop through the random indices and activate the corresponding items
        for (int index = 0; index < rand.Length; index++)
        {
            Item randItem = items[rand[index]];

            // 3. In case of max level item, replace it with a consumable item
            if (randItem.level == randItem.data.damages.Length)
            {
                // Activate the consumable item
                items[4].gameObject.SetActive(true);
            }
            else
            {
                // Activate the randomly selected item
                randItem.gameObject.SetActive(true);
            }
        }
    }
}
