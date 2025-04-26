using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wwy
{
    public class EnemyStats : CharacterStats
    {
        EnemyManager enemyManager;
        public int soulsAwardedOnDeath = 50;
        EnemyAnimatorManager enemyAnimatorManager;
        EnemyBossManager enemyBossManager;

        public UIEnemyHealthBar enemyHealthBar;

        public bool isBoss;
        private void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            enemyBossManager = GetComponent<EnemyBossManager>();
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }
        // Start is called before the first frame update
        void Start()
        {
            if(!isBoss)
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
            if (!isBoss)
            {
                enemyHealthBar.SetHealth(currentHealth);
            }
            else if (isBoss && enemyBossManager != null)
            {
                enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
            }

            if (currentHealth <= 0)
            {
                HandleDeath();
            }
        }
        public override void TakeDamage(int damage, string damageAnimation = "Damage_01")
        {
            base.TakeDamage(damage, damageAnimation);

            if (!isBoss)
            {
                enemyHealthBar.SetHealth(currentHealth);
            }
            else if(isBoss && enemyBossManager != null)
            {
                enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
            }
            enemyAnimatorManager.PlayTargetAnimation(damageAnimation, true);

            if (currentHealth <= 0)
            {
                HandleDeath();
            }
        }
        public void BreakGuard()
        {
            enemyAnimatorManager.PlayTargetAnimation("Break Guard", true);
        }
        public void HandleDeath()
        {
            currentHealth = 0;
            enemyAnimatorManager.PlayTargetAnimation("Dead_01", true);
            isDead = true;
            //Scan for every player in the scene, award the souls

        }

        public override void HandlePoiseResetTimer()
        {
            base.HandlePoiseResetTimer();
        }
    }
}
