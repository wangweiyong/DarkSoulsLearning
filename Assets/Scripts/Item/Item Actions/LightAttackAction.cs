using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    [CreateAssetMenu(menuName = "Item Actions/ Light Attack Action")]
    public class LightAttackAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if (player.playerStatsManager.currentStamina <= 0) return;
            //play FX
            player.playerEffectsManager.PlayWeaponFX(false);

            //if we can perform a running attack, we do that. if not continue
            if (player.isSprinting)
            {
                //handle running attack
                HandleRunningAttack(player);
                return;
            }

            //if we can do a combo, do combo
            if (player.canDoCombo)
            {
                player.inputHandler.comboFlag = true;
                HandleLightWeaponCombo( player);
                player.inputHandler.comboFlag = false;
            }
            else
            {
                if (player.isInteracting) return;
                if (player.canDoCombo) return;
                HandleLigthAttack(player);
            }

        }


        private void HandleLigthAttack(PlayerManager player)
        {
            if (player.isUsingLeftHand)
            {
                player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_light_attack_01, true, false, true);
                player.playerCombatManager.lastAttack = player.playerCombatManager.oh_light_attack_01;

            }
            else if (player.isUsingRightHand)
            {
                if (player.inputHandler.twoHandFlag)
                {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_light_attack_01, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.th_light_attack_01;
                }
                else
                {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_light_attack_01, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.oh_light_attack_01;

                }
            }
        }


        public void HandleRunningAttack(PlayerManager player)
        {



            if (player.isUsingLeftHand)
            {
                player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_running_attack_01, true, false, true);
                player.playerCombatManager.lastAttack = player.playerCombatManager.oh_running_attack_01;
            }
            else if (player.isUsingRightHand)
            {
                if (player.inputHandler.twoHandFlag)
                {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_running_attack_01, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.th_running_attack_01;
                }
                else
                {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_running_attack_01, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.oh_running_attack_01;

                }
            }


        }

        public void HandleLightWeaponCombo(PlayerManager player)
        {

            if (player.inputHandler.comboFlag)
            {
                player.playerAnimatorManager.animator.SetBool("CanDoCombo", false);

                if (player.isUsingLeftHand)
                {
                    if (player.playerCombatManager.lastAttack == player.playerCombatManager.oh_light_attack_01)
                    {
                        player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_light_attack_02, true);
                        player.playerCombatManager.lastAttack = player.playerCombatManager.oh_light_attack_02;
                    }
                    else if (player.playerCombatManager.lastAttack == player.playerCombatManager.oh_light_attack_02)
                    {
                        player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_light_attack_03, true);
                        player.playerCombatManager.lastAttack = player.playerCombatManager.oh_light_attack_03;
                    }
                    else if (player.playerCombatManager.lastAttack == player.playerCombatManager.oh_light_attack_03)
                    {
                        player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_light_attack_04, true);
                        player.playerCombatManager.lastAttack = player.playerCombatManager.oh_light_attack_04;
                    }
                    else if (player.playerCombatManager.lastAttack == player.playerCombatManager.th_heavy_attack_01)
                    {
                        player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_heavy_attack_02, true);
                        player.playerCombatManager.lastAttack = player.playerCombatManager.th_heavy_attack_02;
                    }
                }
                else if (player.isUsingRightHand)
                {
                    if (player.isTwoHandingWeapon)
                    {

                    }
                    else
                    {
                        if (player.playerCombatManager.lastAttack == player.playerCombatManager.oh_light_attack_01)
                        {
                            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_light_attack_02, true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.oh_light_attack_02;
                        }
                        else if (player.playerCombatManager.lastAttack == player.playerCombatManager.oh_light_attack_02)
                        {
                            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_light_attack_03, true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.oh_light_attack_03;
                        }
                        else if (player.playerCombatManager.lastAttack == player.playerCombatManager.oh_light_attack_03)
                        {
                            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.oh_light_attack_04, true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.oh_light_attack_04;
                        }
                        else if (player.playerCombatManager.lastAttack == player.playerCombatManager.th_heavy_attack_01)
                        {
                            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.th_heavy_attack_02, true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.th_heavy_attack_02;
                        }
                    }
                }


            }
        }
    }
}
