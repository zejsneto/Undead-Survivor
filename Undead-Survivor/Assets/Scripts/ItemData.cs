using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scriptable object representing an item's data
[CreateAssetMenu(fileName = "Item", menuName = "Scriptble Object/ItemData")]
public class ItemData : ScriptableObject
{
    // Enum for different item types
    public enum ItemType { Melee, Range, Glove, Shoe, Heal }

    [Header("# Main Info")]
    public ItemType itemType;   // Type of the item
    public int itemId;          // Unique ID of the item
    public string itemName;     // Name of the item
    [TextArea]
    public string itemDesc;     // Description of the item
    public Sprite itemIcon;     // Icon for the item

    [Header("# Level Data")]
    public float baseDamage;    // Base damage of the item
    public int baseCount;       // Base count of the item
    public float[] damages;     // Array of damage values for different levels
    public int[] counts;        // Array of count values for different levels

    [Header("# Weapon")]
    public GameObject projectile;   // Projectile object for ranged weapons
    public Sprite hand;             // Sprite for the hand holding the weapon
}
