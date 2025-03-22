using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wwy
{
    public class DamageCollider : MonoBehaviour
    {
        public CharacterManager characterManager;
        Collider damageCollider;
        public int currentWeaponDamage = 25;
        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider .gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
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
            Debug.Log("Detect Collider" + collision.name);
            if(collision.tag == "Player")
            {
                PlayerStats playerStats = collision.GetComponent<PlayerStats>();

                CharacterManager enemyCharacterManager = collision.GetComponent<CharacterManager>();
                if(enemyCharacterManager != null)
                {
                    if (enemyCharacterManager.isParrying)
                    {
                        //check here if you are parriable
                        characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
                        return;
                    }
                }
                
                if(playerStats != null )
                {
                    playerStats.TakeDamage(currentWeaponDamage);

                }
            }
            else if (collision.tag == "Enemy")
            {
                EnemyStats enemyStats = collision.GetComponent<EnemyStats>();
                
                CharacterManager enemyCharacterManager = collision.GetComponent<CharacterManager>();
                if (enemyCharacterManager != null)
                {
                    if (enemyCharacterManager.isParrying)
                    {
                        //check here if you are parriable
                        characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
                        return;
                    }
                }
                if (enemyStats != null )
                {
                    enemyStats.TakeDamage(currentWeaponDamage);
                }
            }
        }
    }
}
