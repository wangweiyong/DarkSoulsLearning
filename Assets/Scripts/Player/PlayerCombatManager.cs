using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

        [Header("Attack Animations")]
        string oh_light_attack_01 = "OH_Light_Attack_01";
        string oh_light_attack_02 = "OH_Light_Attack_02";
        string oh_light_attack_03 = "OH_Light_Attack_03";
        string oh_light_attack_04 = "OH_Light_Attack_04";
        string oh_heavy_attack_01 = "OH_Heavy_Attack_01";
        string oh_heavy_attack_02 = "OH_Heavy_Attack_02";
        string th_light_attack_01 = "TH_Light_Attack_01";
        string th_light_attack_02 = "TH_Light_Attack_02";
        string th_heavy_attack_01 = "TH_Heavy_Attack_01";
        string th_heavy_attack_02 = "TH_Heavy_Attack_02";
        string weapon_art = "Weapon_Art";

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
                if (lastAttack == oh_light_attack_01)
                {
                    playerAnimatorHandler.PlayTargetAnimation(oh_light_attack_02, true);
                    lastAttack = oh_light_attack_02;
                }
                else if(lastAttack == oh_light_attack_02)
                {
                    playerAnimatorHandler.PlayTargetAnimation(oh_light_attack_03, true);
                    lastAttack = oh_light_attack_03;
                }
                else if(lastAttack == oh_light_attack_03)
                {
                    playerAnimatorHandler.PlayTargetAnimation(oh_light_attack_04, true);
                    lastAttack = oh_light_attack_04;
                }
                else if(lastAttack == th_heavy_attack_01)
                {
                    playerAnimatorHandler.PlayTargetAnimation(th_heavy_attack_02, true);
                    lastAttack = th_heavy_attack_02;
                }
            }
        }
        public void HandleLigthAttack(WeaponItem weapon)
        {
            if (playerStatsManager.currentStamina <= 0)
            {
                return;
            }
            if (weapon != null && !string.IsNullOrEmpty(oh_light_attack_01))
            {
                playerWeaponSlotManger.attackingWeapon = weapon;

                if (inputHandler.twoHandFlag)
                {
                    playerAnimatorHandler.PlayTargetAnimation(th_light_attack_01, true);
                    lastAttack = th_light_attack_01;
                }
                else
                {
                    playerAnimatorHandler.PlayTargetAnimation(oh_light_attack_01, true);
                    lastAttack = oh_light_attack_01;

                }
            }
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            if (playerStatsManager.currentStamina <= 0)
            {
                return;
            }
            if (weapon != null && !string.IsNullOrEmpty(oh_heavy_attack_01))
            {
                playerWeaponSlotManger.attackingWeapon = weapon;


                if (inputHandler.twoHandFlag)
                {
                    playerAnimatorHandler.PlayTargetAnimation(th_heavy_attack_01, true);
                    lastAttack = th_heavy_attack_01;
                }
                else
                {

                    playerAnimatorHandler.PlayTargetAnimation(oh_heavy_attack_01, true);
                    lastAttack = oh_heavy_attack_01;

                }
            }

        }

        #region Input Actions
        public void HandleRBAction()
        {
            if (playerInventoryManager.rightWeapon.weaponType == WeaponType.StraightSword
                || playerInventoryManager.rightWeapon.weaponType == WeaponType.Unarmed)
            {
                PerformRBMeleeAction();
            }
            else if (playerInventoryManager.rightWeapon.weaponType == WeaponType.SpellCaster || playerInventoryManager.rightWeapon.weaponType == WeaponType.PyromancyCaster || playerInventoryManager.rightWeapon.weaponType == WeaponType.FaithCaster)
            {
                PerformMagicAction(playerInventoryManager.rightWeapon, true);
                playerAnimatorHandler.animator.SetBool("isUsingLeftHand", true);
            }
        }

        public void HandleLTAction()
        {
            if (playerInventoryManager.leftWeapon.weaponType == WeaponType.Shield||
                playerInventoryManager.rightWeapon.weaponType == WeaponType.Unarmed)
            {
                PerformLTWeaponArt(inputHandler.twoHandFlag);
            }
            else if (playerInventoryManager.leftWeapon.weaponType == WeaponType.StraightSword)
            {
                //do a light attack
            }
        }
        public void HandleLBAction()
        {
            if (playerManager.isTwoHandingWeapon)
            {
                if(playerInventoryManager.rightWeapon.weaponType == WeaponType.Bow)
                {
                    //aim the bow
                    PerformLBAimingAction();
                }

            }
            else
            {
                if(playerInventoryManager.leftWeapon.weaponType == WeaponType.Shield ||
                    playerInventoryManager.leftWeapon.weaponType == WeaponType.StraightSword)
                {
                    PerformLBBlockingAction();
                }
                else if (playerInventoryManager.leftWeapon.weaponType == WeaponType.FaithCaster ||
   playerInventoryManager.leftWeapon.weaponType == WeaponType.PyromancyCaster)
                {
                    PerformMagicAction(playerInventoryManager.leftWeapon, true);
                }
            }
        }
        private void PerformLBAimingAction()
        {
            //playerAnimatorHandler.animator.SetBool("isAiming", true);
        }
        public void HandleHoldRBAction()
        {
            if (playerManager.isTwoHandingWeapon)
            {
                //do a ranged attack(hold the arrow)
                PerformRBRangedAction();
            }
            else
            {
                //do a melee attack(bow bash)
            }
        }
        private void DrawArrowAction()
        {
            playerAnimatorHandler.animator.SetBool("isHoldingArrow", true);
            playerAnimatorHandler.PlayTargetAnimation("Bow_TH_Draw_01", true);
            GameObject loadedArrow = Instantiate(playerInventoryManager.currentAmmo.loadedItemModel, playerWeaponSlotManger.leftHandSlot.transform);
            //animate the bow 
            Animator bowAnimator = playerWeaponSlotManger.rightHandSlot.GetComponentInChildren<Animator>();
            bowAnimator.SetBool("isDrawn", true);
            bowAnimator.Play("BOW_TH_DRAW_01");
            playerEffectsManager.currentRangedFX = loadedArrow;
        }

        public void FireArrowAction()
        {
            //create live arrow at specific location
            //give ammo velocity
            //destory previous loaded arrow fx
            //set live arrow damage
            //animate the bow firing the arrow
            ArrowInstantiationLocotion arrowInstantiationLocotion;
            arrowInstantiationLocotion = playerWeaponSlotManger.rightHandSlot.GetComponentInChildren<ArrowInstantiationLocotion>();

            Animator bowAnimator = playerWeaponSlotManger.rightHandSlot.GetComponentInChildren<Animator>();
            bowAnimator.SetBool("isDrawn", false);
            bowAnimator.Play("BOW_TH_FIRE_01");
            Destroy(playerEffectsManager.currentRangedFX); // destroy the loaded arrow model

            //reset the player holding arrow flag
            playerAnimatorHandler.PlayTargetAnimation("BOW_TH_FIRE_01", true);
            playerAnimatorHandler.animator.SetBool("isHoldingArrow", false);

            GameObject liveArrow = Instantiate(playerInventoryManager.currentAmmo.liveAmmoMode, arrowInstantiationLocotion.transform.position, cameraHandler.cameraPivotTransform.rotation);
            Rigidbody rigidBody = liveArrow.GetComponentInChildren<Rigidbody>();
            RangedProjectileDamageCollider damageCollider = liveArrow.GetComponentInChildren<RangedProjectileDamageCollider>();

            if(cameraHandler.currentLockOnTarget != null)
            {
                //since while locked we are always facing our target we can copy our facing direction to our arrow facing when fired
                Quaternion arrowRotation = Quaternion.LookRotation(transform.forward);
                liveArrow.transform.rotation = arrowRotation;
            }
            else
            {
                liveArrow.transform.rotation = Quaternion.Euler(cameraHandler.cameraPivotTransform.eulerAngles.x, playerManager.lockOnTransform.eulerAngles.y, 0);
            }
            rigidBody.AddForce(liveArrow.transform.forward * playerInventoryManager.currentAmmo.forwardVelocity);
            rigidBody.AddForce(liveArrow.transform.up * playerInventoryManager.currentAmmo.upwardVelocity);
            rigidBody.useGravity = playerInventoryManager.currentAmmo.useGravity;
            rigidBody.mass = playerInventoryManager.currentAmmo.ammoMass;
            liveArrow.transform.parent = null;

            damageCollider.characterManager = playerManager;
            damageCollider.ammoItem = playerInventoryManager.currentAmmo;
            damageCollider.physicalDamage = playerInventoryManager.currentAmmo.physicalDamage;
        }
        private void PerformRBRangedAction()
        {
            if (playerStatsManager.currentStamina <= 0) return;

            //playerAnimatorHandler.EraseHandIKForWeapon();
            playerAnimatorHandler.animator.SetBool("isUsingRightHand", true);

            if (!playerManager.isHoldingArrow)
            {
                //if we have ammo
                //drain the arrow
                //fire the arrow when we release RB
                //otherwise play an animation to indicate we are out of ammo
                if(playerInventoryManager.currentAmmo != null)
                {
                    //draw the arrow
                    DrawArrowAction();
                }
                else
                {
                    playerAnimatorHandler.PlayTargetAnimation("Shrug", true);
                }
            }
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
                playerAnimatorHandler.PlayTargetAnimation(weapon_art, true);
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
        private void PerformMagicAction(WeaponItem weapon, bool isLeftHanded)
        {
            if (playerManager.isInteracting)
            {
                return;
            }
            if (weapon.weaponType == WeaponType.FaithCaster)
            {
                if(playerInventoryManager.currentSpell !=null && playerInventoryManager.currentSpell.isFaithSpell)
                {
                    //check for fp
                    if(playerStatsManager.currentFocusPoints >= playerInventoryManager.currentSpell.focusPointCost)
                    {
                        playerInventoryManager.currentSpell.AttempToCastSepll(playerAnimatorHandler, playerStatsManager, playerWeaponSlotManger, isLeftHanded);
                    }
                    else
                    {
                        playerAnimatorHandler.PlayTargetAnimation("Shrug", true);
                    }
                }
            }
            else if(weapon.weaponType == WeaponType.PyromancyCaster)
            {
                if (playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isPyroSpell)
                {
                    //check for fp
                    if (playerStatsManager.currentFocusPoints >= playerInventoryManager.currentSpell.focusPointCost)
                    {
                        playerInventoryManager.currentSpell.AttempToCastSepll(playerAnimatorHandler, playerStatsManager, playerWeaponSlotManger,isLeftHanded);
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
            playerInventoryManager.currentSpell.SuccessfullyCastSpell(playerAnimatorHandler, playerStatsManager, cameraHandler, playerWeaponSlotManger, playerManager.isUsingLeftHand);
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

                    int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                    playerAnimatorHandler.PlayTargetAnimation("Back Stab", true);
                    enemyCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Back Stabbed", true);



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

                    int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                    playerAnimatorHandler.PlayTargetAnimation("Riposte", true);
                    enemyCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Riposted", true);
                }
            }
        }
    }
}
