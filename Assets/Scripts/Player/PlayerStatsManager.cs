using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
namespace wwy
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        public float statiminaRenerationAmount = 30f;
        float staminaRegenerationTimer = 0;
        PlayerManager playerManager;

        PlayerAnimatorManager playerAnimationManager;

        public HealthBar healthBar;
        StaminaBar staminaBar;
        FocusPointBar focusPointBar;
        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();
            playerManager = GetComponent<PlayerManager>();
            healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            focusPointBar = FindAnyObjectByType<FocusPointBar>();
            playerAnimationManager = GetComponent<PlayerAnimatorManager>();
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

        public override void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
        {

            if (playerManager.isInvulnerable) return;
            if (isDead)
            {
                return;
            }
            base.TakeDamageNoAnimation(physicalDamage, fireDamage);
            healthBar.SetCurrentHealth(currentHealth);

        }
        public override void TakeDamage(int physicalDamage, int fireDamage,string damageAnimation = "Damage_01")
        {
            if (playerManager.isInvulnerable) return;
            
            base.TakeDamage(physicalDamage, fireDamage, damageAnimation);

            healthBar.SetCurrentHealth(currentHealth);

            playerAnimationManager.PlayTargetAnimation(damageAnimation, true);
        
            if(currentHealth <= 0)
            {
                currentHealth = 0;
                playerAnimationManager.PlayTargetAnimation("Dead_01", true);
                isDead = true;
            }
        }
        public override void TakePoisonDamage(int damage)
        {
            if (isDead) return;

            base.TakePoisonDamage(damage);
            healthBar.SetCurrentHealth(currentHealth);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                playerAnimationManager.PlayTargetAnimation("Dead_01", true);
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

        public override void HandlePoiseResetTimer()
        {
            if(poiseResetTimer > 0)
            {
                poiseResetTimer = poiseResetTimer - Time.deltaTime;
            }
            else if(poiseResetTimer <= 0 && !playerManager.isInteracting)
            {
                totalPoiseDefense = armorPoiseBonus;
            }
        }
    }
}
