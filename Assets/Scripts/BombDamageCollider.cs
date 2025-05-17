using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    public class BombDamageCollider : DamageCollider
    {
        [Header("Explosive Damage & Radius")]
        public int eplosiveRadius = 1;
        public int explosionDamage;
        public int explosionSplashDamage;

        //mageicExlposionDamage
        //lightExlposionDamage

        public Rigidbody rigidBody;
        private bool hasCollided = false;
        public GameObject impactParticles;

        protected override void Awake()
        {
            damageCollider = GetComponent<Collider>();
            rigidBody = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!hasCollided)
            {
                hasCollided = true;
                impactParticles = Instantiate(impactParticles, transform.position, Quaternion.identity);
                Explode();

                CharacterStatsManager character =collision.transform.GetComponent<CharacterStatsManager>();
                if(character != null)
                {
                    //check for friendly fire
                    if(character.teamIDNumber != teamIDNumber)
                    {
                        character.TakeDamage(0, explosionDamage, currentDamageAnimation);
                    }
                }

                Destroy(impactParticles, 5);
                Destroy(gameObject);
            }
        }

        private void Explode()
        {
            Collider[] characters = Physics.OverlapSphere(transform.position, eplosiveRadius);

            foreach (Collider objectInExplosion in characters)
            {
                CharacterStatsManager character = objectInExplosion.GetComponent<CharacterStatsManager>();
                if(character != null)
                {
                    //deal fire damage
                    if(character.teamIDNumber != teamIDNumber)
                    {
                        character.TakeDamage(0, explosionSplashDamage, currentDamageAnimation);
                    }
                }
            }
        }
    }
}
