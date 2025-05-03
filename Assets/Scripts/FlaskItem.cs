using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace wwy
{
    [CreateAssetMenu(menuName = "Item/Consumable/Flask")]
    public class FlaskItem : ConsumableItem
    {
        [Header("Flask Type")]
        public bool estusFlask;

        public bool ashenFlas;

        [Header("Recovery Amount")]
        public int healthRecoverAmount;
        public int focusPointRecoverAmount;

        [Header("Recovery FX")]
        public GameObject recoveryFX;

        public override void AttempToConsumeItem(PlayerAnimatorManager playerAnimatorManager, PlayerWeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
        {
            base.AttempToConsumeItem(playerAnimatorManager, weaponSlotManager, playerEffectsManager);
            // add health or fp
            //instantiate flask in hand and play drink without being hit
            // play recover FX when/ if we drint without being hit
            GameObject flask = Instantiate(itemModel, weaponSlotManager.rightHandSlot.transform);
            playerEffectsManager.currentParticleFX = recoveryFX;
            playerEffectsManager.amountToHeal = healthRecoverAmount;
            playerEffectsManager.instantiatedFXModel = flask;
            weaponSlotManager.rightHandSlot.UnloadWeapon();
        }
    }
}