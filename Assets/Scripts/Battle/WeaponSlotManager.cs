using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wwy
{
    public class WeaponSlotManager : MonoBehaviour
    {
        PlayerManager playerManager;
        PlayerInventory playerInventory;
        public WeaponHolderSlots leftHandSlot;
        public WeaponHolderSlots rightHandSlot;
        WeaponHolderSlots backSlot;

        public DamageCollider leftHandDamageCollider;
        public DamageCollider rightHandDamageCollider;

        public WeaponItem attackingWeapon;

        Animator animator;
        QuickSlotsUI quickSlotsUI;

        PlayerStats playerStats;

        InputHandler inputHandler;
        private void Awake()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            animator = GetComponent<Animator>();
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
            playerStats = GetComponentInParent<PlayerStats>();
            inputHandler = GetComponentInParent<InputHandler>();

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
            LoadWeaponOnSlot(playerInventory.rightWeapon, false);
            LoadWeaponOnSlot(playerInventory.leftWeapon, true);
        }
        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (isLeft)
            {
                leftHandSlot.currentWeaponItem = weaponItem;
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                #region Handle Left Weapon Idle Animation
                if (weaponItem != null)
                {
                    animator.CrossFade(weaponItem.left_hand_idle, 0.2f);
                }
                else
                {
                    animator.CrossFade("Left_Arm_Empty", 0.2f);
                }
                #endregion
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
                    #region Handle Right Weapon Idle Animation
                    animator.CrossFade("Both_Arm_Empty", 0.2f);

                    backSlot.UnloadWeaponAndDestroy();
                    if (weaponItem != null)
                    {
                        animator.CrossFade(weaponItem.right_hand_idle, 0.2f);
                    }
                    else
                    {
                        animator.CrossFade("Right_Arm_Empty", 0.2f);
                    }
                    #endregion
                }
                rightHandSlot.currentWeaponItem = weaponItem;
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
            }
        }

        #region Handle Stamina Drainage
        public void DrainStaminaLightAttack()
        {
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
        }

        public void DrainStaminaHeavyAttack()
        {
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
        }
        #endregion

        #region Handle Weapon's Damage Collider

        private void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            leftHandDamageCollider.currentWeaponDamage = playerInventory.leftWeapon.baseDamage;
            leftHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
            leftHandDamageCollider.poiseBreak = playerInventory.leftWeapon.poiseBreak;
        }
        private void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            rightHandDamageCollider.currentWeaponDamage = playerInventory.rightWeapon.baseDamage;
            rightHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
            rightHandDamageCollider.poiseBreak = playerInventory.rightWeapon.poiseBreak;
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
            playerStats.totalPoiseDefense += attackingWeapon.offensivePoiseBonus;
        }

        public void ResetWeaponAttackingPoiseBonus()
        {
            playerStats.totalPoiseDefense = playerStats.armorPoiseBonus;
        }
        #endregion
    }
}
