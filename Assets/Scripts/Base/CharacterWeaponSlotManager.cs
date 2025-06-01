using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wwy
{
    public class CharacterWeaponSlotManager : MonoBehaviour
    {
        protected CharacterManager characterManager;
        protected CharacterInventoryManager characterInventoryManager;
        protected CharacterAnimatorManager characterAnimatorManager;
        protected CharacterStatsManager characterStatsManager;
        protected CharacterEffectsManager characterEffectsManager;

        [Header("Unarmed Weapon")]
        public WeaponItem unarmedWeapon;
        [Header("Weapon Slots")]
        public WeaponHolderSlots leftHandSlot;
        public WeaponHolderSlots rightHandSlot;
        public WeaponHolderSlots backSlot;

        [Header("Damage Collider")]
        public DamageCollider leftHandDamageCollider;
        public DamageCollider rightHandDamageCollider;

        [Header("Hand IK Target")]
        public RightHandIKTarget rightHandIKTarget;
        public LeftHandIKTarget leftHandIKTarget;

        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
            characterInventoryManager = GetComponent<CharacterInventoryManager>();
            characterEffectsManager = GetComponent<CharacterEffectsManager>();
            characterStatsManager = GetComponent<CharacterStatsManager>();
            LoadWeaponHolderSlots();

        }
        protected virtual void LoadWeaponHolderSlots()
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
        public virtual void LoadWeaponsOnBothHands()
        {
            if (characterInventoryManager.rightWeapon != null)
            {
                LoadWeaponOnSlot(characterInventoryManager.rightWeapon, false);
            }
            if (characterInventoryManager.leftWeapon != null)
            {
                LoadWeaponOnSlot(characterInventoryManager.leftWeapon, true);
            }
        }
        public virtual void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (weaponItem != null)
            {
                if (isLeft)
                {
                    leftHandSlot.currentWeaponItem = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                }
                else 
                {
                    if (characterManager.isTwoHandingWeapon)
                    {
                        //Move current left hand weapon to the back or disable it
                        backSlot.LoadWeaponModel(leftHandSlot.currentWeaponItem);
                        leftHandSlot.UnloadWeaponAndDestroy();
                        characterAnimatorManager.PlayTargetAnimation("Left_Arm_Empty", false, true);
                    }
                    else
                    {
                        backSlot.UnloadWeaponAndDestroy();
                    }
                    rightHandSlot.currentWeaponItem = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                    characterAnimatorManager.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
            else
            {
                weaponItem = unarmedWeapon;
                if (isLeft)
                {
                    characterInventoryManager.leftWeapon = unarmedWeapon;
                    leftHandSlot.currentWeaponItem = unarmedWeapon;
                    leftHandSlot.LoadWeaponModel(unarmedWeapon);
                    LoadLeftWeaponDamageCollider();
                    characterAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    characterInventoryManager.rightWeapon = unarmedWeapon;
                    rightHandSlot.currentWeaponItem = unarmedWeapon;
                    rightHandSlot.LoadWeaponModel(unarmedWeapon);
                    LoadRightWeaponDamageCollider();
                    characterAnimatorManager.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
        }
        protected virtual void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            leftHandDamageCollider.physicalDamage = characterInventoryManager.leftWeapon.physicalDamage;
            leftHandDamageCollider.fireDamage = characterInventoryManager.leftWeapon.fireDamage;
            leftHandDamageCollider.characterManager = characterManager;
            leftHandDamageCollider.poiseBreak = characterInventoryManager.leftWeapon.poiseBreak;
            leftHandDamageCollider.teamIDNumber = characterStatsManager.teamIDNumber;
            characterEffectsManager.leftWeaponFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }
        protected virtual void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            rightHandDamageCollider.physicalDamage = characterInventoryManager.rightWeapon.physicalDamage;
            rightHandDamageCollider.fireDamage = characterInventoryManager.rightWeapon.fireDamage;
            rightHandDamageCollider.characterManager = characterManager;
            rightHandDamageCollider.poiseBreak = characterInventoryManager.rightWeapon.poiseBreak;
            rightHandDamageCollider.teamIDNumber = characterStatsManager.teamIDNumber;
            characterEffectsManager.rightWeaponFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }
        public virtual void LoadTwoHandIKTarget(bool isTwoHandingWeapon)
        {
            leftHandIKTarget = leftHandSlot.currentWeaponModel.GetComponentInChildren<LeftHandIKTarget>();
            rightHandIKTarget = rightHandSlot.currentWeaponModel.GetComponentInChildren<RightHandIKTarget>();

            characterAnimatorManager.SetHandIKForWeapon(rightHandIKTarget, leftHandIKTarget, isTwoHandingWeapon);
        }
        public virtual void OpenDamageCollider()
        {
            if (characterManager.isUsingLeftHand)
            {
                leftHandDamageCollider.EnableDamageCollider();
            }
            else if (characterManager.isUsingRightHand)
            {
                rightHandDamageCollider.EnableDamageCollider();
            }
        }

        public virtual void CloseDamageCollider()
        {
            if (rightHandDamageCollider != null)
            {
                rightHandDamageCollider.DisableDamageCollider();
            }
            if (leftHandDamageCollider != null)
            {
                leftHandDamageCollider.DisableDamageCollider();
            }
        }
        public virtual void GrantWeaponAttackingPoiseBonus()
        {
            WeaponItem currentWeaponBeingUsed = characterInventoryManager.currentItemBeingUsed as WeaponItem;
            characterStatsManager.totalPoiseDefense += currentWeaponBeingUsed.offensivePoiseBonus;
        }

        public virtual void ResetWeaponAttackingPoiseBonus()
        {
            characterStatsManager.totalPoiseDefense = characterStatsManager.armorPoiseBonus;
        }
    }
}
