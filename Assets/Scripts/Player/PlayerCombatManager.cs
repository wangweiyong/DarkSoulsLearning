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
        public string oh_light_attack_01 = "OH_Light_Attack_01";
        public string oh_light_attack_02 = "OH_Light_Attack_02";
        public string oh_light_attack_03 = "OH_Light_Attack_03";
        public string oh_light_attack_04 = "OH_Light_Attack_04";
        public string oh_heavy_attack_01 = "OH_Heavy_Attack_01";
        public string oh_heavy_attack_02 = "OH_Heavy_Attack_02";
        public string th_light_attack_01 = "TH_Light_Attack_01";
        public string th_light_attack_02 = "TH_Light_Attack_02";
        public string th_heavy_attack_01 = "TH_Heavy_Attack_01";
        public string th_heavy_attack_02 = "TH_Heavy_Attack_02";
        public string oh_running_attack_01 = "OH_Running_Attack_01";
        public string oh_jumping_attack_01 = "OH_Jumping_Attack_01";
        public string th_running_attack_01 = "TH_Running_Attack_01";
        public string th_jumping_attack_01 = "TH_Jumping_Attack_01";

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
        
        private void SuccessfullyCastSpell()
        {
            playerInventoryManager.currentSpell.SuccessfullyCastSpell(playerAnimatorHandler, playerStatsManager, cameraHandler, playerWeaponSlotManger, playerManager.isUsingLeftHand);
            playerAnimatorHandler.animator.SetBool("isFiringSpell", true);
        }

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
