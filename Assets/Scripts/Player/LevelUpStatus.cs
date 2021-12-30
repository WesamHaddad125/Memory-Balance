using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LevelUpStatus : MonoBehaviour
{
    public int level = 1;
    public float experience { get; private set; }
    public TMP_Text lvlText;
    public Image expBarImage;
    public Stats playerStats;
    public HeroCombat heroCombat;

    private void Awake()
    {
        lvlText = FindObjectOfType<TMP_Text>();
        expBarImage = GameObject.FindGameObjectWithTag("Exp Bar").GetComponent<Image>();
    }

    public static int ExpNeedToLvlUp(int currLvl)
    {
        if (currLvl == 0)
            return 0;

        return (currLvl * currLvl + currLvl) * 5;
        
    }

    // Calculate experience needed to level up again, more exp needed after each level up
    public void SetExperience(float exp)
    {
        experience += exp;

        float expNeeded = ExpNeedToLvlUp(level);
        float previousExperience = ExpNeedToLvlUp(level - 1);

        // Level Up with Exp
        if (experience >= expNeeded)
        {
            // Once you have enough exp then begin level up
            LevelUp();
            expNeeded = ExpNeedToLvlUp(level);
            previousExperience = ExpNeedToLvlUp(level - 1);
        }

        // Fill Exp Bar Image
        expBarImage.fillAmount = (experience - previousExperience) / (expNeeded - previousExperience);

        // Reset fill bar
        if (expBarImage.fillAmount == 1)
            expBarImage.fillAmount = 0;

    }

    // Increase values of player stats to make them stronger, and update Text level on HUD
    public void LevelUp()
    {
        level++;
        playerStats.maxHealth += 10;
        playerStats.attackDmg += 2;
        heroCombat.regenAmount += 0.3f;
        lvlText.text = level.ToString("");

    }
}
