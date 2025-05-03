using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wwy
{
    public class PlayerCombatManager : MonoBehaviour
    {
        CameraHandler cameraHandler;
        PlayerAnimatorManager playerAnimatorHandler;
        PlayerInventoryManager playerInventoryManager;
        PlayerEquipmentManager playerEquipmentManager;
        PlayerStatsManager playerStatsManager;
        PlayerManager playerManager;
        InputHandler inputHandler;
        PlayerWeaponSlotManager playerWeaponSlotManger;
        PlayerEffectsManager playerEffectsManager;

        public string lastAttack;

        LayerMask backStabLayer = 1 << 13;
        LayerMask riposteLayer = 1 << 14;
        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerManager = GetComponent<PlayerManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerAnimatorHandler = GetComponent<PlayerAnimatorManager>();
            playerWeaponSlotManger = GetComponent<PlayerWeaponSlotManager>();
            inputHandler = GetComponent<InputHandler>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (playerStatsManager.currentStamina <= 0)
            {
                return;
            }
            if (inputHandler.comboFlag)
            {
                playerAnimatorHandler.animator.SetBool("CanDoCombo", false);
                if (lastAttack == weapon.OH_Light_Attack_1)
                {
                    playerAnimatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_2, true);
                    lastAttack = weapon.OH_Light_Attack_2;
                }
                else if(lastAttack == weapon.OH_Light_Attack_2)
                {
                    playerAnimatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_3, true);
                    lastAttack = weapon.OH_Light_Attack_3;
                }
                else if(lastAttack == weapon.OH_Light_Attack_3)
                {
                    playerAnimatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_4, true);
                    lastAttack = weapon.OH_Light_Attack_4;
                }
                else if(lastAttack == weapon.TH_Light_Attack_1)
                {
                    playerAnimatorHandler.PlayTargetAnimation(weapon.TH_Light_Attack_2, true);
                    lastAttack = weapon.TH_Light_Attack_2;
                }
            }
        }
        public void HandleLigthAttack(WeaponItem weapon)
        {
            if (playerStatsManager.currentStamina <= 0)
            {
                return;
            }
            if (weapon != null && !string.IsNullOrEmpty(weapon.OH_Light_Attack_1))
            {
                playerWeaponSlotManger.attackingWeapon = weapon;

                if (inputHandler.twoHandFlag)
                {
                    playerAnimatorHandler.PlayTargetAnimation(weapon.TH_Light_Attack_1, true);
                    lastAttack = weapon.TH_Light_Attack_1;
                }
                else
                {
                    playerAnimatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
                    lastAttack = weapon.OH_Light_Attack_1;

                }
            }
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            if (playerStatsManager.currentStamina <= 0)
            {
                return;
            }
            if (weapon != null && !string.IsNullOrEmpty(weapon.OH_Heavy_Attack_1))
            {
                playerWeaponSlotManger.attackingWeapon = weapon;


                if (inputHandler.twoHandFlag)
                {
                    playerAnimatorHandler.PlayTargetAnimation(weapon.TH_Heavy_Attack_1, true);
                    lastAttack = weapon.TH_Heavy_Attack_1;
                }
                else
                {

                    playerAnimatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
                    lastAttack = weapon.OH_Heavy_Attack_1;

                }
            }

        }

        #region Input Actions
        public void HandleRBAction()
        {
            if (playerInventoryManager.rightWeapon.isMeleeWeapon)
            {
                PerformRBMeleeAction();
            }
            else if (playerInventoryManager.rightWeapon.isSpellCaster || playerInventoryManager.rightWeapon.isPyroCaster || playerInventoryManager.rightWeapon.isFaithCaster)
            {
                PerformRBMagicAction(playerInventoryManager.rightWeapon);
            }
        }

        public void HandleLTAction()
        {
            if (playerInventoryManager.leftWeapon.isShield)
            {
                PerformLTWeaponArt(inputHandler.twoHandFlag);
            }
            else if (playerInventoryManager.leftWeapon.isMeleeWeapon)
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
                playerAnimatorHandler.PlayTargetAnimation(playerInventoryManager.leftWeapon.weapon_art, true);
            }
        }
        #endregion

        #region Attack Actions
        private void PerformRBMeleeAction()
        {
            if (playerManager.canDoCombo)
            {
                inputHandler.comboFlag = true;
                HandleWeaponCombo(playerInventoryManager.rightWeapon);
                inputHandler.comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting) return;
                if (playerManager.canDoCombo) return;
                playerAnimatorHandler.animator.SetBool("isUsingRightHand", true);
                HandleLigthAttack(playerInventoryManager.rightWeapon);
            }

            //play FX
            playerEffectsManager.PlayWeaponFX(false);
        }
        private void PerformRBMagicAction(WeaponItem weapon)
        {
            if (playerManager.isInteracting)
            {
                return;
            }
            if (weapon.isFaithCaster)
            {
                if(playerInventoryManager.currentSpell !=null && playerInventoryManager.currentSpell.isFaithSpell)
                {
                    //check for fp
                    if(playerStatsManager.currentFocusPoints >= playerInventoryManager.currentSpell.focusPointCost)
                    {
                        playerInventoryManager.currentSpell.AttempToCastSepll(playerAnimatorHandler, playerStatsManager, playerWeaponSlotManger);
                    }
                    else
                    {
                        playerAnimatorHandler.PlayTargetAnimation("Shrug", true);
                    }
                }
            }
            else if(weapon.isPyroCaster)
            {
                if (playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isPyroSpell)
                {
                    //check for fp
                    if (playerStatsManager.currentFocusPoints >= playerInventoryManager.currentSpell.focusPointCost)
                    {
                        playerInventoryManager.currentSpell.AttempToCastSepll(playerAnimatorHandler, playerStatsManager, playerWeaponSlotManger);
                    }
                    else
                    {
                        playerAnimatorHandler.PlayTargetAnimation("Shrug", true);
                    }
                }
            }
        }
        
        private void SuccessfullyCastSpell()
        {
            playerInventoryManager.currentSpell.SuccessfullyCastSpell(playerAnimatorHandler, playerStatsManager, cameraHandler, playerWeaponSlotManger);
            playerAnimatorHandler.animator.SetBool("isFiringSpell", true);
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

            playerAnimatorHandler.PlayTargetAnimation("Blocking Start", false, true);
            playerEquipmentManager.OpenBlockingCollider();
            playerManager.isBlocking = true;

        }
        #endregion
        public void AttemptBackStabOrRiposte()
        {
            if (playerStatsManager.currentStamina <= 0)
            {
                return;
            }
            RaycastHit hit;
            if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
            {
                CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponent<CharacterManager>();
                DamageCollider rightWeapon = playerWeaponSlotManger.rightHandDamageCollider;
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

                    int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                    playerAnimatorHandler.PlayTargetAnimation("Back Stab", true);
                    enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Back Stabbed", true);



                }
            }
            else if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 0.7f, riposteLayer))
            {
                CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponent<CharacterManager>();
                DamageCollider rightWeapon = playerWeaponSlotManger.rightHandDamageCollider;
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

                    int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                    playerAnimatorHandler.PlayTargetAnimation("Riposte", true);
                    enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Riposted", true);
                }
            }
        }
    }
}
