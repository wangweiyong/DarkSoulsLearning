using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;

    public int staminaLevel = 10;
    public float maxStamina;
    public float currentStamina;

    public int focusLevel = 10;
    public float maxFocusPoints;
    public float currentFocusPoints;


    public int soulCount = 0;
    [Header("Armor Absorption")]
    public float physicalDamageAbsorptionHead;
    public float physicalDamageAbsorptionBody;
    public float physicalDamageAbsorptionLegs;
    public float physicalDamageAbsorptionHands;
    //Fire Absorption
    //Lighting Absorption
    //Magic Absorption
    //Dark Absorption

    public bool isDead;

    public virtual void TakeDamage(int physicalDamage, string damageAnimation = "Damage_01")
    {
        if(isDead) return;
        float totalDamgePhysicalDamageAbsorption = 1 - (1 - physicalDamageAbsorptionHead / 100) * (1 - physicalDamageAbsorptionBody / 100)
            * (1 - physicalDamageAbsorptionLegs / 100) * (1 - physicalDamageAbsorptionHands / 100);
        Debug.Log("totalDamgePhysicalDamageAbsorption" + totalDamgePhysicalDamageAbsorption);
        physicalDamage = Mathf.RoundToInt( physicalDamage - physicalDamage * totalDamgePhysicalDamageAbsorption);
        int finalDamage = physicalDamage; //+ fireDamage + lightingDamage + darkDamage
        Debug.Log("finalDamage" + finalDamage);
        currentHealth = currentHealth - finalDamage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
    }
}
