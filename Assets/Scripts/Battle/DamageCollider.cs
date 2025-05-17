using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wwy
{
    public class DamageCollider : MonoBehaviour
    {
        public CharacterManager characterManager;
        protected Collider damageCollider;
        public bool enableDamageColliderOnStartUp = false;

        [Header("Team I.D")]
        public int teamIDNumber = 0;

        [Header("Damage")]
        public int physicalDamage = 25;
        public int fireDamage = 25;
        public int magicDamage = 25;
        public int lightingDamage = 25;
        public int darkDamage = 25;

        [Header("Poise")]
        public float poiseBreak;
        public float offensivePoiseBonus;

        protected virtual void Awake()
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
            if(collision.tag == "Character")
            {
                CharacterStatsManager enemyStats = collision.GetComponent<CharacterStatsManager>();

                CharacterManager enemyCharacterManager = collision.GetComponent<CharacterManager>();
                CharacterEffectsManager enemyEffectManager = collision.GetComponent<CharacterEffectsManager>();
                BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();
                if(enemyCharacterManager != null)
                {
                    if (enemyStats.teamIDNumber == teamIDNumber) return;
                    if (enemyCharacterManager.isParrying)
                    {
                        //check here if you are parriable
                        characterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Parried", true);
                        return;
                    }
                    else if(shield != null && enemyCharacterManager.isBlocking)
                    {
                        float physicalDamageAfterBlock = physicalDamage - (physicalDamage * shield.blockingPhysicalDamageAbsorption) / 100;
                        float fireDamageAfterBlock = fireDamage - (fireDamage * shield.blockingFireDamageAbsorption) / 100;
                        if(enemyStats != null)
                        {
                            enemyStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), Mathf.RoundToInt(fireDamageAfterBlock), "Block Guard");
                            return;
                        }
                    }
                }
                
                if(enemyStats != null )
                {
                    if (enemyStats.teamIDNumber == teamIDNumber) return;

                    enemyStats.poiseResetTimer = enemyStats.totalPoiseResettime;
                    enemyStats.totalPoiseDefense = enemyStats.totalPoiseDefense - poiseBreak;

                    Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);// detect our weapons where on the collider first contact
                    enemyEffectManager.PlayBlookdSplatterFX(contactPoint);
                    if (enemyStats.totalPoiseDefense > poiseBreak)
                    {
                        enemyStats.TakeDamageNoAnimation(physicalDamage, 0);
                    }
                    else
                    {
                        enemyStats.TakeDamage(physicalDamage, 0);
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
