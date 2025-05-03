using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wwy
{
    public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
    {
        PlayerManager playerManager;
        PlayerInventoryManager playerInventoryManager;
        Animator animator;
        QuickSlotsUI quickSlotsUI;
        PlayerStatsManager playerStatsManager;
        InputHandler inputHandler;
        PlayerEffectsManager playerEffectsManager;

        [Header("Attacking Weapon")]
        public WeaponItem attackingWeapon;



        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            animator = GetComponentInChildren<Animator>();
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            inputHandler = GetComponent<InputHandler>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();

            LoadWeaponHolderSlots();
        }
        private void LoadWeaponHolderSlots()
        {
            WeaponHolderSlots[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlots>();
            foreach (WeaponHolderSlots weaponSlot in weaponHolderSlots)
            {
                if (weaponSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponSlot;
                }
                else if (weaponSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponSlot;
                }
                else if (weaponSlot.isBackSlot)
                {
                    backSlot = weaponSlot;
                }
            }
        }
        public void LoadBothWeaponsOnSlot()
        {
            LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
            LoadWeaponOnSlot(playerInventoryManager.leftWeapon, true);
        }
        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if(weaponItem != null)
            {
                if (isLeft)
                {
                    leftHandSlot.currentWeaponItem = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                    animator.CrossFade(weaponItem.left_hand_idle, 0.2f);

                    
                }
                else if (!isLeft)
                {
                    if (inputHandler.twoHandFlag)
                    {
                        //Move current left hand weapon to the back or disable it
                        backSlot.LoadWeaponModel(leftHandSlot.currentWeaponItem);
                        leftHandSlot.UnloadWeaponAndDestroy();
                        animator.CrossFade(weaponItem.th_idle, 0.2f);
                    }
                    else
                    {
                        animator.CrossFade("Both_Arm_Empty", 0.2f);

                        backSlot.UnloadWeaponAndDestroy();

                        animator.CrossFade(weaponItem.right_hand_idle, 0.2f);

    
                   
                    }
                    rightHandSlot.currentWeaponItem = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
                }
            }
            else
            {
                weaponItem = unarmedWeapon;
                if (isLeft)
                {
                    animator.CrossFade("Left_Arm_Empty", 0.2f);
                    playerInventoryManager.leftWeapon = unarmedWeapon;
                    leftHandSlot.currentWeaponItem = unarmedWeapon;
                    leftHandSlot.LoadWeaponModel(unarmedWeapon);
                    LoadLeftWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(true, unarmedWeapon);
                }
                else
                {
                    animator.CrossFade("Right_Arm_Empty", 0.2f);
                    playerInventoryManager.rightWeapon = unarmedWeapon;
                    rightHandSlot.currentWeaponItem = unarmedWeapon;
                    rightHandSlot.LoadWeaponModel(unarmedWeapon);
                    LoadRightWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(false, unarmedWeapon);
                }
            }
        }

        #region Handle Stamina Drainage
        public void DrainStaminaLightAttack()
        {
            playerStatsManager.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
        }

        public void DrainStaminaHeavyAttack()
        {
            playerStatsManager.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
        }
        #endregion

        #region Handle Weapon's Damage Collider

        private void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            leftHandDamageCollider.currentWeaponDamage = playerInventoryManager.leftWeapon.baseDamage;
            leftHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
            leftHandDamageCollider.poiseBreak = playerInventoryManager.leftWeapon.poiseBreak;
            playerEffectsManager.leftWeaponFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }
        private void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            rightHandDamageCollider.currentWeaponDamage = playerInventoryManager.rightWeapon.baseDamage;
            rightHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
            rightHandDamageCollider.poiseBreak = playerInventoryManager.rightWeapon.poiseBreak;
            playerEffectsManager.rightWeaponFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }
        public void OpenDamageCollider()
        {
            if (playerManager.isUsingLeftHand)
            {
                leftHandDamageCollider.EnableDamageCollider();
            }
            else if(playerManager.isUsingRightHand)
            {
                rightHandDamageCollider.EnableDamageCollider();
            }
        }

        public void CloseDamageCollider()
        {
            if (rightHandDamageCollider != null)
            {
                rightHandDamageCollider.DisableDamageCollider();
            }
            if(leftHandDamageCollider != null)
            {
                leftHandDamageCollider.DisableDamageCollider();
            }
        }

        #endregion

        #region Handle Weapon's Poise Bonus
        public void GrantWeaponAttackingPoiseBonus()
        {
            playerStatsManager.totalPoiseDefense += attackingWeapon.offensivePoiseBonus;
        }

        public void ResetWeaponAttackingPoiseBonus()
        {
            playerStatsManager.totalPoiseDefense = playerStatsManager.armorPoiseBonus;
        }
        #endregion
    }
}
