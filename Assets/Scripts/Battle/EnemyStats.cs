using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wwy
{
    public class EnemyStats : CharacterStats
    {
        public int soulsAwardedOnDeath = 50;
        EnemyAnimatorManager enemyAnimatorManager;

        public UIEnemyHealthBar enemyHealthBar;
        private void Awake()
        {
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }
        // Start is called before the first frame update
        void Start()
        {
            enemyHealthBar.SetMaxHealth(maxHealth);
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamageNoAnimation(int damage)
        {
            if (isDead) return;
            currentHealth = currentHealth - damage;
            enemyHealthBar.SetHealth(currentHealth);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }
        public override void TakeDamage(int damage, string damageAnimation = "Damage_01")
        {
            base.TakeDamage(damage, damageAnimation);
            currentHealth = currentHealth - damage;
            enemyHealthBar.SetHealth(currentHealth);

            enemyAnimatorManager.PlayTargetAnimation(damageAnimation, true);

            if (currentHealth <= 0)
            {
                HandleDeath();
            }
        }
        public void HandleDeath()
        {
            currentHealth = 0;
            enemyAnimatorManager.PlayTargetAnimation("Dead_01", true);
            isDead = true;
            //Scan for every player in the scene, award the souls

        }
    }
}
