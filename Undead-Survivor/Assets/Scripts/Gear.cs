using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    // Type of gear (Glove or Shoe)
    public ItemData.ItemType type;

    // Rate of the gear
    public float rate;

    // Method to initialize the gear with item data
    public void Init(ItemData data)
    {
        // Basic Set
        name = "Gear" + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        type = data.itemType;
        rate = data.damages[0];
        // Apply the gear effects
        ApplyGear();
    }

    // Method to level up the gear
    public void LevelUp(float rate)
    {
        this.rate = rate;
        // Apply the gear effects
        ApplyGear();
    }

    // Method to apply the gear effects based on its type
    private void ApplyGear()
    {
        switch (type)
        {
            case ItemData.ItemType.Glove:
                RateUp();   // Increase attack rate
                break;
            case ItemData.ItemType.Shoe:
                SpeedUp();  // Increase movement speed
                break;
        }
    }

    // Method to increase attack rate
    private void RateUp()
    {
        // Get all weapons attached to the player
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            float speed; // Variable to store weapon speed

            switch (weapon.id)
            {
                case 0: // If weapon is melee
                    speed = 150 * Character.WeaponSpeed;
                    weapon.speed = speed + (150 * rate); // Increase rotation speed
                    break;
                default: // If weapon is ranged
                    speed = 0.5f * Character.WeaponRate;
                    weapon.speed = speed * (1f - rate); // Decrease fire rate
                    break;
            }
        }
    }

    // Method to increase movement speed
    void SpeedUp()
    {
        float speed = 3 * Character.Speed; // Base movement speed
        GameManager.instance.player.speed = speed + speed * rate; // Increase movement speed
    }
}
