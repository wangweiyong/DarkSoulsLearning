using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
namespace wwy
{
    public class PlayerStats : CharacterStats
    {
        public float statiminaRenerationAmount = 30f;
        float staminaRegenerationTimer = 0;
        PlayerManager playerManager;

        PlayerAnimatorManager animatorHandler;

        HealthBar healthBar;
        StaminaBar staminaBar;
        FocusPointBar focusPointBar;
        // Start is called before the first frame update
        private void Awake()
        {
            playerManager= GetComponent<PlayerManager>();
            healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            focusPointBar = FindAnyObjectByType<FocusPointBar>();
            animatorHandler = GetComponentInChildren<PlayerAnimatorManager>();
        }
        void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);

            maxStamina = SetMaxStaminaFromStaminaLevel();
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina);

            maxFocusPoints = SetMaxFocusPointsFromFocusLevel();
            currentFocusPoints = maxFocusPoints;
            focusPointBar.SetMaxFocusPoint(maxFocusPoints);
            focusPointBar.SetCurrentFocusPoint(currentFocusPoints);
        }

        public void HealPlayer(int healAmount)
        {
            currentHealth += healAmount;
            if(currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            healthBar.SetCurrentHealth(currentHealth);
        }
        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        private float SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;
        }

        private float SetMaxFocusPointsFromFocusLevel()
        {
            maxFocusPoints = focusLevel * 10;
            return maxFocusPoints;
        }

        public void TakeDamageNoAnimation(int damage)
        {

            if (playerManager.isInvulerable) return;
            if (isDead)
            {
                return;
            }
            currentHealth = currentHealth - damage;
            healthBar.SetCurrentHealth(currentHealth);
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }
        public override void TakeDamage(int damage, string damageAnimation = "Damage_01")
        {
            if (playerManager.isInvulerable) return;
            if (isDead)
            {
                return;
            }
            currentHealth = currentHealth - damage;

            healthBar.SetCurrentHealth(currentHealth);

            animatorHandler.PlayTargetAnimation(damageAnimation, true);
        
            if(currentHealth <= 0)
            {
                currentHealth = 0;
                animatorHandler.PlayTargetAnimation("Dead_01", true);
                isDead = true;
            }
        }
    
        public void TakeStaminaDamage(int damage)
        {

            currentStamina = currentStamina - damage;
            //Set Bar
            staminaBar.SetCurrentStamina(currentStamina);
        }
        
        public void RegenerateStamina()
        {
            if (playerManager.isInteracting)
            {
                staminaRegenerationTimer = 0;
            }
            else
            {
                staminaRegenerationTimer += Time.deltaTime;
                if (currentStamina < maxStamina && staminaRegenerationTimer > 1f)
                {
                    currentStamina += statiminaRenerationAmount * Time.deltaTime;
                    staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                }
            }
        }
    
        public void DeductFocusPoints(int focusPoints)
        {
            currentFocusPoints -= focusPoints;
            if(currentFocusPoints < 0)
            {
                currentFocusPoints = 0;
            }
            focusPointBar.SetCurrentFocusPoint(currentFocusPoints);
        }


        public void AddSoulds(int souls)
        {
            soulCount = soulCount + souls;
        }
    }
}
