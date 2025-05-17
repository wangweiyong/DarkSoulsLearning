using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    [CreateAssetMenu(menuName = "Item/Consumable/Cure Effect Clump")]
    public class ClumpConsumableItem : ConsumableItem
    {
        [Header("Recovery FX")]
        public GameObject clumpConsumeFX;

        [Header("Cure FX")]
        public bool curePoison;
        //cure bleed
        //cure curse
        public override void AttempToConsumeItem(PlayerAnimatorManager playerAnimatorManager, PlayerWeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
        {
            base.AttempToConsumeItem(playerAnimatorManager, weaponSlotManager, playerEffectsManager);
            // add health or fp
            //instantiate flask in hand and play drink without being hit
            // play recover FX when/ if we drint without being hit
            GameObject clump = Instantiate(itemModel, weaponSlotManager.rightHandSlot.transform);
            playerEffectsManager.currentParticleFX = clumpConsumeFX;
            playerEffectsManager.instantiatedFXModel = clump;
            if (curePoison)
            {
                playerEffectsManager.poisonBuildUp = 0;
                playerEffectsManager.poisonAmount = playerEffectsManager.defaultPoisonAmount;
                playerEffectsManager.isPoisoned = false;
                if(playerEffectsManager.currentPoisonParticleFX != null)
                {
                    Destroy(playerEffectsManager.currentPoisonParticleFX);
                }
            }
            weaponSlotManager.rightHandSlot.UnloadWeapon();
        }
    }
}
