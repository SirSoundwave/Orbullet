using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private Image healthStats;
    [SerializeField]
    private Image ammoStats;

    public float maxAmmo = 50;
    public float maxHealth = 50;

    /* Subtracts damage from fill amount based on damage taken and max Health. */
    public void displayHealthStats(float damage)
    {
        float healthValue = damage / maxHealth;
        healthStats.fillAmount -= healthValue;
        healthStats.fillAmount = (float)Math.Round(healthStats.fillAmount, 4);
    }

    public void displayAmmoStats(int shots)
    {
        float ammoValue = shots / maxAmmo;
        ammoStats.fillAmount -= ammoValue;
        ammoStats.fillAmount = (float)Math.Round(ammoStats.fillAmount, 4);
    }

    public void fillAmmo()
    {
        ammoStats.fillAmount = 1;
    }
    public void fillAmmo(int rounds)
    {
        float ammoValue = rounds / maxAmmo;
        ammoStats.fillAmount += ammoValue;
        ammoStats.fillAmount = (float)Math.Round(ammoStats.fillAmount, 4);
        if (ammoStats.fillAmount > 1)
        {
            ammoStats.fillAmount = 1;
        }
    }
    public void fillHealth()
    {
        ammoStats.fillAmount = 1;
    }

    public void fillHealth(int hp)
    {
        float healthValue = hp / maxHealth;
        healthStats.fillAmount += healthValue;
        healthStats.fillAmount = (float)Math.Round(healthStats.fillAmount, 4);
        if(healthStats.fillAmount > 1)
        {
            healthStats.fillAmount = 1;
        }
    }

    public Image getHealth()
    {
        return healthStats;
    }

    public Image getAmmo()
    {
        return ammoStats;
    }

}
