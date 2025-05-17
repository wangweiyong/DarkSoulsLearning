using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        CharacterStatsManager characterStatsManager;
        [Header("Damage FX")]
        public GameObject bloodSplatterFX;
        [Header("Weapon FX")]
        public WeaponFX rightWeaponFX;
        public WeaponFX leftWeaponFX;

        [Header("Poison")]
        public GameObject defaultPoisonParticleFX;
        public GameObject currentPoisonParticleFX;
        public Transform buildUpTransform;
        public bool isPoisoned;
        public float poisonBuildUp = 0; //build up over time that poisons the player after reaching 100
        public float poisonAmount = 100;//the amount of poison the player has to process before becoming unpoisoned
        public float defaultPoisonAmount = 100;//the default amount of poison a player has to process once they become poisoned
        public int poisonDamage = 1;
        public float poisonTimer = 2;
        float timer;

        protected virtual void Awake()
        {
            characterStatsManager = GetComponent<CharacterStatsManager>();
        }
        public virtual void PlayWeaponFX(bool isLeft)
        {
            if(isLeft == false)
            {
                //play the right weapons trail
                if(rightWeaponFX != null)
                {
                    rightWeaponFX.PlayWeaponFX();
                }
            }
            else
            {
                //play the left weapons trail
                if(leftWeaponFX != null)
                {
                    leftWeaponFX.PlayWeaponFX();
                }
            }
        }
        
        public virtual void PlayBlookdSplatterFX(Vector3 blookSplatterLocation)
        {
            if(bloodSplatterFX != null)
            {
                GameObject blood = Instantiate(bloodSplatterFX, blookSplatterLocation, Quaternion.identity);
            }
        }
    
        public virtual void HandleAllBuildUpEffects()
        {
            if (characterStatsManager.isDead) return;
            HandlePoisonBuildUp();
            HandleIsPoisonedEffect();
        }
        protected virtual void HandlePoisonBuildUp()
        {
            if (isPoisoned)
            {
                return;
            }
            if(poisonBuildUp > 0 && poisonBuildUp < 100)
            {
                poisonBuildUp = poisonBuildUp - 1 * Time.deltaTime;
            }
            else if(poisonBuildUp >= 100)
            {
                isPoisoned = true;
                poisonBuildUp = 0;
                if (defaultPoisonParticleFX != null)
                {
                    if (buildUpTransform != null)
                    {
                        currentPoisonParticleFX = Instantiate(defaultPoisonParticleFX, buildUpTransform.transform);
                    }
                    else
                    {
                        currentPoisonParticleFX = Instantiate(defaultPoisonParticleFX, characterStatsManager.transform);
                    }
                }
            }
        }
    
        protected virtual void HandleIsPoisonedEffect()
        {
            if (isPoisoned)
            {
                if(poisonAmount > 0)
                {
                    timer += Time.deltaTime;
                    if(timer >= poisonTimer)
                    {
                        characterStatsManager.TakePoisonDamage(poisonDamage);
                        timer = 0;
                    }
                    //damage player
                    poisonAmount = poisonAmount - 1 * Time.deltaTime;
                }
                else
                {
                    isPoisoned = false;
                    poisonAmount = defaultPoisonAmount;
                    Destroy(currentPoisonParticleFX);
                }
            }
        }
    }
}
