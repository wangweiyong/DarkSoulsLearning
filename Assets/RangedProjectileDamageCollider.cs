using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    public class RangedProjectileDamageCollider : DamageCollider
    {
        public RangedAmmoItem ammoItem;
        protected bool hasAlreadyPenetratedSurface;
        protected GameObject penetratedProjectile;
        protected override void OnTriggerEnter(Collider collision)
        {
            if (collision.tag == "Character")
            {
                shieldHasBeenHit = false;
                hasBeenParried = false;
                CharacterStatsManager enemyStats = collision.GetComponent<CharacterStatsManager>();

                CharacterManager enemyCharacterManager = collision.GetComponent<CharacterManager>();
                CharacterEffectsManager enemyEffectManager = collision.GetComponent<CharacterEffectsManager>();
                BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();
                if (enemyCharacterManager != null)
                {
                    if (enemyStats.teamIDNumber == teamIDNumber) return;
                    CheckForParry(enemyCharacterManager);
                    CheckForBlock(enemyCharacterManager, shield, enemyStats);
                }

                if (enemyStats != null)
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
    }
}
