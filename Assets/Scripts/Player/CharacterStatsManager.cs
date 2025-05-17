using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
namespace wwy {
    public class CharacterStatsManager : MonoBehaviour
    {
        CharacterAnimatorManager characterAnimatorManager;
        [Header("Team I.D")]
        public int teamIDNumber = 0;


        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;

        public int staminaLevel = 10;
        public float maxStamina;
        public float currentStamina;

        public int focusLevel = 10;
        public float maxFocusPoints;
        public float currentFocusPoints;
        public int soulsAwardedOnDeath = 50;


        public int soulCount = 0;

        [Header("Poise")]
        public float totalPoiseDefense;//total poise during damage caclulation
        public float offensivePoiseBonus;// the poise you gain during an attack with a weapon
        public float armorPoiseBonus;//the poise you gain from wearing whatever you have equipped
        public float totalPoiseResettime = 15;
        public float poiseResetTimer = 0;

        [Header("Armor Absorption")]
        public float physicalDamageAbsorptionHead;
        public float physicalDamageAbsorptionBody;
        public float physicalDamageAbsorptionLegs;
        public float physicalDamageAbsorptionHands;
        //Fire Absorption
        //Lighting Absorption
        //Magic Absorption
        //Dark Absorption
        public float fireDamageAbsorptionHead;
        public float fireDamageAbsorptionBody;
        public float fireDamageAbsorptionLegs;
        public float fireDamageAbsorptionHands;


        public bool isDead;
        protected virtual void Awake()
        {
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        }
        public virtual void TakeDamage(int physicalDamage, int fireDamage, string damageAnimation)
        {
            if (isDead) return;
            //characterAnimatorManager.EraseHandIKForWeapon();
            float totalDamgePhysicalDamageAbsorption = 1 - (1 - physicalDamageAbsorptionHead / 100) * (1 - physicalDamageAbsorptionBody / 100)
                * (1 - physicalDamageAbsorptionLegs / 100) * (1 - physicalDamageAbsorptionHands / 100);

            float totalDamgeFireDamageAbsorption = 1 - (1 - fireDamageAbsorptionHead / 100) * (1 - fireDamageAbsorptionBody / 100)
    * (1 - fireDamageAbsorptionLegs / 100) * (1 - fireDamageAbsorptionHands / 100);
            physicalDamage = Mathf.RoundToInt(physicalDamage - physicalDamage * totalDamgePhysicalDamageAbsorption);
            fireDamage = Mathf.RoundToInt(fireDamage - fireDamage * totalDamgeFireDamageAbsorption);
            int finalDamage = physicalDamage + fireDamage; //+ lightingDamage + darkDamage
            currentHealth = currentHealth - finalDamage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }

        public virtual void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
        {
            if (isDead) return;
            float totalDamgePhysicalDamageAbsorption = 1 - (1 - physicalDamageAbsorptionHead / 100) * (1 - physicalDamageAbsorptionBody / 100)
                * (1 - physicalDamageAbsorptionLegs / 100) * (1 - physicalDamageAbsorptionHands / 100);

            float totalDamgeFireDamageAbsorption = 1 - (1 - fireDamageAbsorptionHead / 100) * (1 - fireDamageAbsorptionBody / 100)
    * (1 - fireDamageAbsorptionLegs / 100) * (1 - fireDamageAbsorptionHands / 100);
            physicalDamage = Mathf.RoundToInt(physicalDamage - physicalDamage * totalDamgePhysicalDamageAbsorption);
            fireDamage = Mathf.RoundToInt(fireDamage - fireDamage * totalDamgeFireDamageAbsorption);
            int finalDamage = physicalDamage + fireDamage; //+ lightingDamage + darkDamage
            currentHealth = currentHealth - finalDamage;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }
        public virtual void TakePoisonDamage(int damage)
        {
            currentHealth = currentHealth - damage;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }
        private void Start()
        {
            totalPoiseDefense = armorPoiseBonus;
        }
        protected virtual void Update()
        {
            HandlePoiseResetTimer();
        }
        public virtual void HandlePoiseResetTimer()
        {
            if(poiseResetTimer > 0)
            {
                poiseResetTimer = poiseResetTimer - Time.deltaTime;
            }
            else
            {
                totalPoiseDefense = armorPoiseBonus;
            }
        }
    }
}