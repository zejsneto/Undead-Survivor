using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;          // ID of the weapon
    public int prefabId;    // Prefab ID of the weapon
    public float damage;    // Damage inflicted by the weapon
    public int count;       // Number of bullets fired per shot
    public float speed;     // Speed of the weapon's rotation or firing rate

    private float timer;            // Timer for controlling weapon firing rate
    private Player player;          // Reference to the player object

    private void Awake()
    {
        player = GameManager.instance.player;
    }

    private void Update()
    {
        // If the player is not alive, do nothing
        if (!GameManager.instance.isAlive)
            return;

        // Perform actions based on weapon ID
        switch (id)
        {
            // ID 0, rotate the weapon
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            // For other IDs, fire bullets at a certain rate
            default:
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    timer = 0;
                    Fire();
                }
                break;
        }

        // Test code: Level up when "Jump" button is pressed
        /*
        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(10, 1);
        }
        */
    }

    // Level up the weapon's damage and count
    public void LevelUp(float damage, int count)
    {
        // Increase damage and count based on input parameters and character stats
        this.damage = damage * Character.Damage;
        this.count += count;

        // If the weapon is of ID 0, perform batch firing
        if (id == 0)
            Batch();

        // Apply gear changes to the player
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    // Initialize the weapon with item data
    public void Init(ItemData data)
    {
        // Set basic properties
        name = "Weapon" + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        // Set properties based on item data and character stats
        id = data.itemId;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;

        // Find the prefab ID of the projectile
        for (int index = 0; index < GameManager.instance.pool.prefabs.Length; index++)
        {
            if (data.projectile == GameManager.instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }

        // Set speed based on weapon type and character stats
        switch (id)
        {
            case 0:
                speed = 150 * Character.WeaponSpeed;
                Batch();
                break;
            default:
                speed = 0.3f * Character.WeaponRate;
                break;
        }

        // Set hand sprite and activate hand object
        Hand hand = player.hands[(int)data.itemType];
        hand.spriteR.sprite = data.hand;
        hand.gameObject.SetActive(true);

        // Apply gear changes to the player
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    // Melee
    private void Batch()
    {
        // Loop through the count and fire bullets accordingly
        for (int index = 0; index < count; index++)
        {
            Transform bullet;

            // Get bullet from the pool or create a new one
            if (index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }

            // Set bullet position and rotation
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            // Rotate bullet around weapon
            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); // -100 is Infinity Per.
        }
    }

    // Range
    private void Fire()
    {
        // If no nearest target is found, do nothing
        if (!player.scanner.nearestTarget)
            return;

        // Calculate direction towards the nearest target
        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        // Get a bullet from the pool and fire it towards the target
        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);

        // Play firing sound effect
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }
}
