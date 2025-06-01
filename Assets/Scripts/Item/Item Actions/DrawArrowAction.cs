using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    [CreateAssetMenu(menuName = "Item Actions/ Draw Arrow Action")]
    public class DrawArrowAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if (player.isInteracting) return;
            if(player.isHoldingArrow) return;
            //animate player
            player.playerAnimatorManager.animator.SetBool("isHoldingArrow", true);
            player.playerAnimatorManager.PlayTargetAnimation("Bow_TH_Draw_01", true);
            //instantiate arrow
            GameObject loadedArrow = Instantiate(player.playerInventoryManager.currentAmmo.loadedItemModel, player.playerWeaponSlotManager.leftHandSlot.transform);
            player.playerEffectsManager.currentRangedFX = loadedArrow;
            //animate the bow 
            Animator bowAnimator = player.playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
            bowAnimator.SetBool("isDrawn", true);
            bowAnimator.Play("BOW_TH_DRAW_01");
        }
    }
}
