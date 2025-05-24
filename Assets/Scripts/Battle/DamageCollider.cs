using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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

        protected bool shieldHasBeenHit;
        protected bool hasBeenParried;
        protected string currentDamageAnimation;

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
        protected virtual void CheckForParry(CharacterManager enemyCharacterManager)
        {
            if (enemyCharacterManager.isParrying)
            {
                //check here if you are parriable
                characterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Parried", true);
                hasBeenParried = true;
            }
        }
        protected virtual void CheckForBlock(CharacterManager enemyCharacterManager, BlockingCollider shield, CharacterStatsManager enemyStatsManager)
        {
            if (shield != null && enemyCharacterManager.isBlocking)
            {
                float physicalDamageAfterBlock = physicalDamage - (physicalDamage * shield.blockingPhysicalDamageAbsorption) / 100;
                float fireDamageAfterBlock = fireDamage - (fireDamage * shield.blockingFireDamageAbsorption) / 100;
                if (enemyStatsManager != null)
                {
                    enemyStatsManager.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), Mathf.RoundToInt(fireDamageAfterBlock), "Block Guard");
                    shieldHasBeenHit = true;
                }
            }
        }
        protected virtual void OnTriggerEnter(Collider collision)
        {
            //Debug.Log("Detect Collider" + collision.name);
            if(collision.tag == "Character")
            {
                shieldHasBeenHit = false;
                hasBeenParried = false;
                CharacterStatsManager enemyStats = collision.GetComponent<CharacterStatsManager>();

                CharacterManager enemyCharacterManager = collision.GetComponent<CharacterManager>();
                CharacterEffectsManager enemyEffectManager = collision.GetComponent<CharacterEffectsManager>();
                BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();
                if(enemyCharacterManager != null)
                {
                    if (enemyStats.teamIDNumber == teamIDNumber) return;
                    CheckForParry(enemyCharacterManager);
                    CheckForBlock(enemyCharacterManager, shield, enemyStats);
                }
                
                if(enemyStats != null )
                {
                    if (enemyStats.teamIDNumber == teamIDNumber) return;
                    if (hasBeenParried) return;
                    if (shieldHasBeenHit) return;
                    enemyStats.poiseResetTimer = enemyStats.totalPoiseResettime;
                    enemyStats.totalPoiseDefense = enemyStats.totalPoiseDefense - poiseBreak;

                    Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);// detect our weapons where on the collider first contact

                    float directionHitFrom = Vector3.SignedAngle(characterManager.transform.forward, enemyCharacterManager.transform.forward, Vector3.up);
                    ChooseWhichDirectionDamageCameFrom(directionHitFrom);
                    enemyEffectManager.PlayBlookdSplatterFX(contactPoint);
                    if (enemyStats.totalPoiseDefense > poiseBreak)
                    {
                        enemyStats.TakeDamageNoAnimation(physicalDamage, 0);
                    }
                    else
                    {
                        enemyStats.TakeDamage(physicalDamage, 0, currentDamageAnimation);
                    }

                }
            }
            else if (collision.tag == "Illusionary Wall")
            {
                IllusionaryWall illusionaryWall = collision.GetComponent<IllusionaryWall>();
                illusionaryWall.wallHasBeenHit = true;
            }
        }
    
        protected virtual void ChooseWhichDirectionDamageCameFrom(float direction)
        {
            if(direction >= 145 && direction <= 180)
            {
                currentDamageAnimation = "Damage_Forward_01";
            }
            else if(direction <= -145 && direction >= -180)
            {
                currentDamageAnimation = "Damage_Forward_01";
            }
            else if(direction >= -45 && direction <= 45)
            {
                currentDamageAnimation = "Damage_Back_01";
            }
            else if(direction >= 144 && direction <= -45)
            {
                currentDamageAnimation = "Damage_Left_01";
            }
            else if(direction >= 45 && direction <= 144)
            {
                currentDamageAnimation = "Damage_Right_01";
            }
        }
    }
}
