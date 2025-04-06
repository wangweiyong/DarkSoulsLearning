using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wwy
{
    public class PlayerAttacker : MonoBehaviour
    {
        PlayerAnimatorManager animatorHandler;
        PlayerInventory playerInventory;
        PlayerStats playerStats;
        PlayerManager playerManager;
        InputHandler inputHandler;
        WeaponSlotManager weaponSlotManger;

        public string lastAttack;

        LayerMask backStabLayer = 1 << 13;
        LayerMask riposteLayer = 1 << 14;
        private void Awake()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            playerStats = GetComponentInParent<PlayerStats>();
            animatorHandler = GetComponent<PlayerAnimatorManager>();
            weaponSlotManger = GetComponent<WeaponSlotManager>();
            inputHandler = GetComponentInParent<InputHandler>();
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (playerStats.currentStamina <= 0)
            {
                return;
            }
            if (inputHandler.comboFlag)
            {
                animatorHandler.anim.SetBool("CanDoCombo", false);
                if (lastAttack == weapon.OH_Light_Attack_1)
                {
                    animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_2, true);
                    lastAttack = weapon.OH_Light_Attack_2;
                }
                else if(lastAttack == weapon.OH_Light_Attack_2)
                {
                    animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_3, true);
                    lastAttack = weapon.OH_Light_Attack_3;
                }
                else if(lastAttack == weapon.OH_Light_Attack_3)
                {
                    animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_4, true);
                    lastAttack = weapon.OH_Light_Attack_4;
                }
                else if(lastAttack == weapon.TH_Light_Attack_1)
                {
                    animatorHandler.PlayTargetAnimation(weapon.TH_Light_Attack_2, true);
                    lastAttack = weapon.TH_Light_Attack_2;
                }
            }
        }
        public void HandleLigthAttack(WeaponItem weapon)
        {
            if (playerStats.currentStamina <= 0)
            {
                return;
            }
            if (weapon != null && !string.IsNullOrEmpty(weapon.OH_Light_Attack_1))
            {
                weaponSlotManger.attackingWeapon = weapon;

                if (inputHandler.twoHandFlag)
                {
                    animatorHandler.PlayTargetAnimation(weapon.TH_Light_Attack_1, true);
                    lastAttack = weapon.TH_Light_Attack_1;
                }
                else
                {
                    animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
                    lastAttack = weapon.OH_Light_Attack_1;

                }
            }
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            if (playerStats.currentStamina <= 0)
            {
                return;
            }
            if (weapon != null && !string.IsNullOrEmpty(weapon.OH_Heavy_Attack_1))
            {
                weaponSlotManger.attackingWeapon = weapon;


                if (inputHandler.twoHandFlag)
                {
                    animatorHandler.PlayTargetAnimation(weapon.TH_Heavy_Attack_1, true);
                    lastAttack = weapon.TH_Heavy_Attack_1;
                }
                else
                {

                    animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
                    lastAttack = weapon.OH_Heavy_Attack_1;

                }
            }

        }

        #region Input Actions
        public void HandleRBAction()
        {
            if (playerInventory.rightWeapon.isMeleeWeapon)
            {
                PerformRBMeleeAction();
            }
            else if (playerInventory.rightWeapon.isSpellCaster || playerInventory.rightWeapon.isPyroCaster || playerInventory.rightWeapon.isFaithCaster)
            {
                PerformRBMagicAction(playerInventory.rightWeapon);
            }
        }

        public void HandleLTAction()
        {
            if (playerInventory.leftWeapon.isShield)
            {
                PerformLTWeaponArt(inputHandler.twoHandFlag);
            }
            else if (playerInventory.leftWeapon.isMeleeWeapon)
            {
                //do a light attack
            }
        }
        public void HandleLBAction()
        {
            PerformLBBlockingAction();
        }
        private void PerformLTWeaponArt(bool isTwoHanding)
        {
            if (playerManager.isInteracting) return;

            //if we are two handing perform right weapon
            //else perform weapon art for left weapon
            if (isTwoHanding)
            {
            }
            else
            {
                animatorHandler.PlayTargetAnimation(playerInventory.leftWeapon.weapon_art, true);
            }
        }
        #endregion

        #region Attack Actions
        private void PerformRBMeleeAction()
        {
            if (playerManager.canDoCombo)
            {
                inputHandler.comboFlag = true;
                HandleWeaponCombo(playerInventory.rightWeapon);
                inputHandler.comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting) return;
                if (playerManager.canDoCombo) return;
                animatorHandler.anim.SetBool("isUsingRightHand", true);
                HandleLigthAttack(playerInventory.rightWeapon);
            }
        }
        private void PerformRBMagicAction(WeaponItem weapon)
        {
            if (playerManager.isInteracting)
            {
                return;
            }
            if (weapon.isFaithCaster)
            {
                if(playerInventory.currentSpell !=null && playerInventory.currentSpell.isFaithSpell)
                {
                    //check for fp
                    if(playerStats.currentFocusPoints >= playerInventory.currentSpell.focusPointCost)
                    {
                        playerInventory.currentSpell.AttempToCastSepll(animatorHandler, playerStats);
                    }
                    else
                    {
                        animatorHandler.PlayTargetAnimation("Shrug", true);
                    }
                }
            }
        }
        
        private void SuccessfullyCastSpell()
        {
            playerInventory.currentSpell.SuccessfullyCastSpell(animatorHandler, playerStats);
        }
        #endregion

        #region Defense Actions
        public void PerformLBBlockingAction()
        {
            if (playerManager.isInteracting)
            {
                return;
            }
            if (playerManager.isBlocking) return;

            animatorHandler.PlayTargetAnimation("Block Start", false, true);
            playerManager.isBlocking = true;

        }
        #endregion
        public void AttemptBackStabOrRiposte()
        {
            if (playerStats.currentStamina <= 0)
            {
                return;
            }
            RaycastHit hit;
            if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
            {
                CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponent<CharacterManager>();
                DamageCollider rightWeapon = weaponSlotManger.rightHandDamageCollider;
                if (enemyCharacterManager != null)
                {
                    //Check for team id so you cannot back stab friends or yourself
                    //pull is into a transform behind the enemy so the backstab looks clean
                    //rotate us towards that transform

                    //play animation
                    //make enemy play animation
                    //do damage
                    playerManager.transform.position = enemyCharacterManager.backStabCollider.criticalDamageStandPoint.position;
                    Vector3 rotationDirection = playerManager.transform.eulerAngles;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 800 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;

                    int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                    animatorHandler.PlayTargetAnimation("Back Stab", true);
                    enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Back Stabbed", true);



                }
            }
            else if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 0.7f, riposteLayer))
            {
                CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponent<CharacterManager>();
                DamageCollider rightWeapon = weaponSlotManger.rightHandDamageCollider;
                if(enemyCharacterManager!=null && enemyCharacterManager.canBeRiposte)
                {
                    playerManager.transform.position = enemyCharacterManager.transform.position;

                    Vector3 rotationDirection = playerManager.transform.eulerAngles;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 800 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;

                    int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                    animatorHandler.PlayTargetAnimation("Riposte", true);
                    enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Riposted", true);
                }
            }
        }
    }
}
