using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data;   // Data of the item
    public int level;       // Current level of the item
    public Weapon weapon;   // Reference to the weapon component
    public Gear gear;       // Reference to the gear component

    private Image icon;     // Reference to the Image component for the item icon
    private Text textLevel; // Reference to the Text component for the item level
    private Text textName;  // Reference to the Text component for the item name
    private Text textDesc;  // Reference to the Text component for the item description

    private void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];
        textName.text = data.itemName;
        textDesc.text = data.itemDesc;
    }

    private void OnEnable()
    {
        // Update item level text
        textLevel.text = "Lv." + (level + 1);

        // Update item description based on item type and level
        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                break;
            default:
                textDesc.text = string.Format(data.itemDesc);
                break;
        }
    }

    // Method called when the item is clicked
    public void OnClick()
    {
        // Handle different types of items
        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                // If the item is at level 0, create a new weapon and initialize it with item data
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                }
                // If the item is at a higher level, level up the existing weapon
                else
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    nextDamage += data.baseDamage * data.damages[level];
                    nextCount += data.counts[level];

                    weapon.LevelUp(nextDamage, nextCount);
                }
                level++;
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                // If the item is at level 0, create a new gear and initialize it with item data
                if (level == 0)
                {
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(data);
                }
                // If the item is at a higher level, level up the existing gear
                else
                {
                    float nextRate = data.damages[level];
                    gear.LevelUp(nextRate);
                }
                level++;
                break;
            case ItemData.ItemType.Heal:
                // Heal the player to maximum health
                GameManager.instance.health = GameManager.instance.maxHealth;
                break;
        }

        // Disable the item button if it has reached the maximum level
        if (level == data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
