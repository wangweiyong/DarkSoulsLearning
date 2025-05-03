using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wwy
{
    public class DamageCollider : MonoBehaviour
    {
        public CharacterManager characterManager;
        Collider damageCollider;
        public bool enableDamageColliderOnStartUp = false;

        [Header("Damage")]
        public int currentWeaponDamage = 25;

        [Header("Poise")]
        public float poiseBreak;
        public float offensivePoiseBonus;

        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider .gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = enableDamageColliderOnStartUp;
        }
        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }
        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }
        private void OnTriggerEnter(Collider collision)
        {
            //Debug.Log("Detect Collider" + collision.name);
            if(collision.tag == "Player")
            {
                PlayerStatsManager playerStats = collision.GetComponent<PlayerStatsManager>();

                CharacterManager playerCharacterManager = collision.GetComponent<CharacterManager>();
                CharacterEffectsManager playerEffectManager = collision.GetComponent<CharacterEffectsManager>();
                BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();
                if(playerCharacterManager != null)
                {
                    if (playerCharacterManager.isParrying)
                    {
                        //check here if you are parriable
                        characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
                        return;
                    }
                    else if(shield != null && playerCharacterManager.isBlocking)
                    {
                        float physicalDamageAfterBlock = currentWeaponDamage - (currentWeaponDamage * shield.blockingPhysicalDamageAbsorption) / 100;
                        if(playerStats != null)
                        {
                            playerStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block Guard");
                            return;
                        }
                    }
                }
                
                if(playerStats != null )
                {
                    playerStats.poiseResetTimer = playerStats.totalPoiseResettime;
                    playerStats.totalPoiseDefense = playerStats.totalPoiseDefense - poiseBreak;

                    Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);// detect our weapons where on the collider first contact
                    playerEffectManager.PlayBlookdSplatterFX(contactPoint);
                    if (playerStats.totalPoiseDefense > poiseBreak)
                    {
                        playerStats.TakeDamageNoAnimation(currentWeaponDamage);
                    }
                    else
                    {
                        playerStats.TakeDamage(currentWeaponDamage);
                    }

                }
            }
            else if (collision.tag == "Enemy")
            {
                EnemyStatsManager enemyStats = collision.GetComponent<EnemyStatsManager>();
                
                CharacterManager enemyCharacterManager = collision.GetComponent<CharacterManager>();
                CharacterEffectsManager enemyEffectManager = collision.GetComponent<CharacterEffectsManager>();

                BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();
                Debug.Log("Enemy's Poise is currently" + enemyStats.totalPoiseDefense);

                if (enemyCharacterManager != null)
                {
                    if (enemyCharacterManager.isParrying)
                    {
                        //check here if you are parriable
                        characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
                        return;
                    }
                    else if (shield != null && enemyCharacterManager.isBlocking)
                    {
                        float physicalDamageAfterBlock = currentWeaponDamage - (currentWeaponDamage * shield.blockingPhysicalDamageAbsorption) / 100;
                        if (enemyStats != null)
                        {
                            enemyStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block Guard");
                            return;
                        }
                    }
                }
                if (enemyStats != null )
                {
                    enemyStats.poiseResetTimer = enemyStats.totalPoiseResettime;
                    enemyStats.totalPoiseDefense = enemyStats.totalPoiseDefense - poiseBreak;
                    Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);// detect our weapons where on the collider first contact
                    enemyEffectManager.PlayBlookdSplatterFX(contactPoint);
                    if (enemyStats.isBoss)
                    {
                        if (enemyStats.totalPoiseDefense > poiseBreak)
                        {
                            enemyStats.TakeDamageNoAnimation(currentWeaponDamage);
                        }
                        else
                        {
                            enemyStats.TakeDamageNoAnimation(currentWeaponDamage);
                            enemyStats.BreakGuard();
                        }
                    }
                    else
                    {
                        if (enemyStats.totalPoiseDefense > poiseBreak)
                        {
                            enemyStats.TakeDamageNoAnimation(currentWeaponDamage);
                        }
                        else
                        {
                            enemyStats.TakeDamage(currentWeaponDamage);
                        }
                    }

                }
            }
            else if (collision.tag == "Illusionary Wall")
            {
                IllusionaryWall illusionaryWall = collision.GetComponent<IllusionaryWall>();
                illusionaryWall.wallHasBeenHit = true;
            }
        }
    }
}
