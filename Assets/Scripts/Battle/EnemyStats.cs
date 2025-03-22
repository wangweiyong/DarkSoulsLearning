using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wwy
{
    public class EnemyStats : CharacterStats
    {
        public int soulsAwardedOnDeath = 50;
        EnemyAnimatorManager enemyAnimatorManager;

        // Start is called before the first frame update
        void Start()
        {
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
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
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }
        public void TakeDamage(int damage)
        {
            if (isDead) return;
            currentHealth = currentHealth - damage;

            enemyAnimatorManager.PlayTargetAnimation("Damage_01", true);

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
