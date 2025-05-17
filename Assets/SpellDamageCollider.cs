using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    public class SpellDamageCollider : DamageCollider
    {
        public GameObject impactParticles;
        public GameObject projectileParticles;
        public GameObject muzzleParticles;

        bool hasCollided = false;
        CharacterStatsManager spellTarget;
        Vector3 impactNormal;//Used to rotate the impact particles
        private void Start()
        {
            poiseBreak = 25;
            if (projectileParticles != null)
            {
                projectileParticles = Instantiate(projectileParticles, transform.position, transform.rotation);
                projectileParticles.transform.parent = transform;
            }

            if (muzzleParticles != null)
            {
                muzzleParticles = Instantiate(muzzleParticles, transform.position, transform.rotation);
                Destroy(muzzleParticles, 2f);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!hasCollided)
            {
                spellTarget = other.transform.GetComponent<CharacterStatsManager>();

                if(spellTarget != null && teamIDNumber != spellTarget.teamIDNumber)
                {
                    EnemyStatsManager enemyStats = spellTarget as EnemyStatsManager;
                    if(enemyStats != null)
                    {
                        enemyStats.poiseResetTimer = enemyStats.totalPoiseResettime;
                        enemyStats.totalPoiseDefense = enemyStats.totalPoiseDefense - poiseBreak;
                        if (enemyStats.totalPoiseDefense > poiseBreak)
                        {
                            enemyStats.TakeDamageNoAnimation(0, fireDamage);
                        }
                        else
                        {
                            enemyStats.TakeDamage(0, fireDamage);
                        }
                    }
                }
                hasCollided = true;
                impactParticles = Instantiate(impactParticles, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal));

                Destroy(projectileParticles);
                Destroy(impactParticles, 5f);
                Destroy(gameObject, 5f);
            }
        }
    }
}