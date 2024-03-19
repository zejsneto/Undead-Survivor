using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    // Enum to define different types of information to display on the HUD
    public enum InfoType { Exp, Level, Kill, Time, Health }

    // Type of information this HUD element displays
    public InfoType type;

    // Reference to the Text component attached to this object
    private Text myText;

    // Reference to the Slider component attached to this object
    private Slider mySlider;

    private void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        // Update HUD based on the type of information it represents
        switch (type)
        {
            case InfoType.Exp:
                // Calculate current and maximum experience values
                float curExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];
                // Set the slider value based on the current and maximum experience
                mySlider.value = curExp / maxExp;
                break;
            case InfoType.Level:
                // Display player level
                myText.text = string.Format("Lv. {0:F0}", GameManager.instance.level);
                break;
            case InfoType.Kill:
                // Display number of kills
                myText.text = string.Format("{0:F0}", GameManager.instance.kill);
                break;
            case InfoType.Time:
                // Calculate remaining time
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);
                // Format and display remaining time as MM:SS
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;
            case InfoType.Health:
                // Calculate current and maximum health values
                float curHealth = GameManager.instance.health;
                float maxHealth = GameManager.instance.maxHealth;
                // Set the slider value based on the current and maximum health
                mySlider.value = curHealth / maxHealth;
                break;
        }
    }
}
