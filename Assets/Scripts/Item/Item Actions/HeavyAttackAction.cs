using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    [CreateAssetMenu(menuName = "Item Actions/ Heavy Attack Action")]
    public class HeavyAttackAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if (player.playerStatsManager.currentStamina <= 0) return;
            //play FX
            player.playerEffectsManager.PlayWeaponFX(false);

            //if we can perform a jumping attack, we do that. if not continue
            if (player.isSprinting)
            {
                //handle jumping attack
                HandleJumpingAttack(player);
                return;
            }

            //if we can do a combo, do combo
            if (player.canDoCombo)
            {
                player.inputHandler.comboFlag = true;
                HandleHeavyWeaponCombo(player);
                player.inputHandler.comboFlag = false;
            }
            else
            {
                if (player.isInteracting) return;
                if (player.canDoCombo) return;
                HandleHeavyAttack(player);
            }

        }
        private void HandleHeavyAttack(PlayerManager player)
        {
            if (player.isUsingLeftHand)
            {
                player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_heavy_attack_01, true, false, true);
                player.playerCombatManager.lastAttack = player.playerCombatManager.oh_heavy_attack_01;
            }
            else if (player.isUsingRightHand)
            {
                if (player.inputHandler.twoHandFlag)
                {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_heavy_attack_01, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.th_heavy_attack_01;
                }
                else
                {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_heavy_attack_01, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.oh_heavy_attack_01;
                }
            }


        }
        public void HandleJumpingAttack(PlayerManager player)
        {
            if (player.isUsingLeftHand)
            {
                player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_jumping_attack_01, true, false, true);
                player.playerCombatManager.lastAttack = player.playerCombatManager.oh_jumping_attack_01;
            }
            else if (player.isUsingRightHand)
            {
                if (player.inputHandler.twoHandFlag)
                {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_jumping_attack_01, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.th_jumping_attack_01;
                }
                else
                {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_jumping_attack_01, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.oh_jumping_attack_01;

                }
            }


        }
        public void HandleHeavyWeaponCombo(PlayerManager player)
        {

            if (player.inputHandler.comboFlag)
            {
                player.playerAnimatorManager.animator.SetBool("CanDoCombo", false);

                if (player.isUsingLeftHand)
                {
                    if (player.playerCombatManager.lastAttack == player.playerCombatManager.oh_heavy_attack_01)
                    {
                        player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_heavy_attack_02, true);
                        player.playerCombatManager.lastAttack = player.playerCombatManager.oh_heavy_attack_02;
                    }
                }
                else if (player.isUsingRightHand)
                {
                    if (player.isTwoHandingWeapon)
                    {

                    }
                    else
                    {
                        if (player.playerCombatManager.lastAttack == player.playerCombatManager.oh_heavy_attack_01)
                        {
                            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_heavy_attack_02, true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.oh_heavy_attack_02;
                        }
                    }
                }
            }
        }
    }
}
